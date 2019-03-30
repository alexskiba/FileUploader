using System.Threading.Tasks;
using FileUploader.Domain;
using NSubstitute;
using NUnit.Framework;

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
    }
}