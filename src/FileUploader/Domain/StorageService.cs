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
            try
            {
                await _storage.Save(_mapper.MapProducts(inputStream));
            }
            catch (CsvHelper.ValidationException e)
            {
                throw new ValidationException(e.Message, e);
            }
        }
    }
}