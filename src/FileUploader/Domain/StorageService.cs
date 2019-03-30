using System.IO;
using System.Threading.Tasks;

namespace FileUploader.Domain
{
    public class StorageService
    {
        private readonly IProductMapper _mapper;
        private readonly IProductStorage _storage;

        public StorageService(IProductMapper mapper, IProductStorage storage)
        {
            _mapper = mapper;
            _storage = storage;
        }

        public async Task Save(Stream inputStream)
        {
            await _storage.Save(await _mapper.MapProducts(inputStream));
        }
    }
}