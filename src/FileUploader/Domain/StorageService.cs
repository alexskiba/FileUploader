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

        /// <summary>
        /// Saves data from the specified input to a configured storage using configured mapper.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="sessionId">Used to avoid data duplication in case of retry. Should be unique per data set.</param>
        /// <returns></returns>
        public async Task Save(Stream inputStream, string sessionId)
        {
            try
            {
                await _storage.Save(_mapper.MapProducts(inputStream), sessionId);
            }
            catch (CsvHelper.ValidationException e)
            {
                throw new ValidationException(e.Message, e);
            }
        }
    }
}