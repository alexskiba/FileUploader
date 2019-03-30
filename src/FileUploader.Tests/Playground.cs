using System.Threading.Tasks;
using Autofac;
using FileUploader.Domain;
using FileUploader.Service;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace FileUploader.Tests
{
    public class Playground
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test()
        {
            var mapperMock = Substitute.For<IProductMapper>();
            var storageMock = Substitute.For<IProductStorage>();
            var storageService = new StorageService(mapperMock, storageMock);

            await storageService.Save(null);
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