using System.Collections.Generic;
using System.IO;

namespace FileUploader.Domain
{
    public interface IProductMapper
    {
        IEnumerable<Product> MapProducts(Stream inputStream);
    }
}