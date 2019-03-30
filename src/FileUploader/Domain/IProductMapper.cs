using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileUploader.Domain
{
    public interface IProductMapper
    {
        Task<IEnumerable<Product>> MapProducts(Stream inputStream);
    }
}