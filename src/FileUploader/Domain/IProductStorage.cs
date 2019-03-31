using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUploader.Domain
{
    public interface IProductStorage
    {
        Task Save(IEnumerable<Product> products, string sessionId);
    }
}