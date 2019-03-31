using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using FileUploader.Domain;
using FileUploader.Service;
using FileUploader.Storage;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using Shouldly;

namespace FileUploader.Tests
{
    public class Playground
    {
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger();
        }

        [Test]
        public async Task Test()
        {
            var mapperMock = Substitute.For<IProductMapper>();
            var storageMock = Substitute.For<IProductStorage>();
            var storageService = new StorageService(mapperMock, storageMock);

            await storageService.Save(null, null);
        }

        [Test]
        public async Task JsonLocalStorage()
        {

            var sessionId = Guid.NewGuid().ToString("N");
            var storageFolder = "tmp";
            var expectedFileName = $"{storageFolder}\\{sessionId}.json";
            var jsonStorage = new LocalStorage(storageFolder);

            await jsonStorage.Save(new[]
                                   {
                                       new Product {ArticleCode = "test", Price = 9},
                                       new Product {ArticleCode = "test_2"}
                                   },
                                   sessionId);

            File.Exists(expectedFileName).ShouldBeTrue();
            var stored = JsonConvert.DeserializeObject<Product[]>(File.ReadAllText(expectedFileName));
            stored.Length.ShouldBe(2);
            stored[0].ArticleCode.ShouldBe("test");
            stored[1].ArticleCode.ShouldBe("test_2");
        }

        [Test]
        public void Autofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacModule>();
            var container = builder.Build();

            var storageService = container.Resolve<StorageService>();

            storageService.ShouldNotBeNull();
        }
    }
}