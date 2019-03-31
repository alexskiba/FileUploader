using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FileUploader.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

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

        public async Task Save(IEnumerable<Product> products, string sessionId)
        {
            var fileName = Path.GetFullPath(Path.Combine(_storageFolder, sessionId + ".json"));
            var productsCount = 0;

            Log.Debug($"Starting saving products stream to {fileName}");
            using (var streamWriter = new StreamWriter(fileName, false))
            {
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    await jsonWriter.WriteStartArrayAsync();

                    try
                    {
                        foreach (var product in products)
                        {
                            JObject.FromObject(product).WriteTo(jsonWriter);
                            productsCount++;
                        }
                    }
                    finally
                    {
                        await jsonWriter.WriteEndArrayAsync();
                        await jsonWriter.FlushAsync();
                        Log.Debug($"Saved {productsCount} products to {fileName}");
                    }
                }
            }
        }
    }
}