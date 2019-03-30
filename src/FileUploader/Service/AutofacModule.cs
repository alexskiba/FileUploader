using Autofac;
using FileUploader.Domain;
using FileUploader.Storage;

namespace FileUploader.Service
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CsvProductMapper>().As<IProductMapper>();
            builder
                .RegisterType<LocalStorage>()
                .As<IProductStorage>()
                .WithParameter(new TypedParameter(typeof(string), "../../LocalStorage"));
            builder.RegisterType<StorageService>();
        }
    }
}