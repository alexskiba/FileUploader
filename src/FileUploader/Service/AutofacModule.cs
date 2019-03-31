using System;
using Autofac;
using FileUploader.Domain;
using FileUploader.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FileUploader.Service
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configuration;

        public AutofacModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            switch (_configuration["ProductStorageType"])
            {
                case "Sql":
                    RegisterSqlStorage(builder, _configuration.GetConnectionString("ProductsDatabase"));
                    break;
                case "Json":
                    RegisterJsonStorage(builder, _configuration["LocalStoragePath"]);
                    break;
                default:
                    throw new ArgumentException("Unknown ProductStorageType");
            }

            builder.RegisterType<CsvProductMapper>().As<IProductMapper>();
            builder.RegisterType<StorageService>();
        }

        private void RegisterSqlStorage(ContainerBuilder builder, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(connectionString);
            builder.RegisterType<ProductsContext>().AsSelf().WithParameter("options", optionsBuilder.Options);
            builder.RegisterType<SqlStorage>().As<IProductStorage>();
        }

        private void RegisterJsonStorage(ContainerBuilder builder, string storageFolder)
        {
            builder
                .RegisterType<LocalStorage>()
                .As<IProductStorage>()
                .WithParameter(new TypedParameter(typeof(string), storageFolder));
        }
    }
}