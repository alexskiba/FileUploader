using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileUploader.Domain;
using Newtonsoft.Json;

namespace FileUploader.Storage
{
    public class LocalStorage : IProductStorage
    {
        private readonly string _storageFolder;

        public LocalStorage(string storageFolder)
        {
            _storageFolder = storageFolder;
            if (!Directory.Exists(_storageFolder))
            {
                Directory.CreateDirectory(_storageFolder);
            }
        }

        public async Task Save(IEnumerable<Product> products)
        {
            var fileName = Path.Combine(_storageFolder, Guid.NewGuid().ToString("N") + ".json");
            await File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(products.ToList()));
        }
    }
}