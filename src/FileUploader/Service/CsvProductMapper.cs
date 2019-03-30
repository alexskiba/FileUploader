using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FileUploader.Domain;

namespace FileUploader.Service
{
    public class CsvProductMapper : IProductMapper
    {
        public async Task<IEnumerable<Product>> MapProducts(Stream inputStream)
        {
            return await Task.FromResult(new List<Product>());
        }
    }
}