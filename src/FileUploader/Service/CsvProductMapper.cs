using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using FileUploader.Domain;

namespace FileUploader.Service
{
    public class CsvProductMapper : IProductMapper
    {
        private readonly Configuration _mappingConfig;

        public CsvProductMapper()
        {
            _mappingConfig = new Configuration();
            _mappingConfig.RegisterClassMap<CsvProductMap>();
        }

        public IEnumerable<Product> MapProducts(Stream inputStream)
        {
            using (var reader = new StreamReader(inputStream))
            {
                using (var csvReader = new CsvReader(reader, _mappingConfig))
                {
                    while (csvReader.Read())
                    {
                        yield return csvReader.GetRecord<Product>();
                    }
                }
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class CsvProductMap : DefaultClassMap<Product>
        {
            public CsvProductMap()
            {
                AutoMap();
                Map(m => m.ArticleCode).Name("ArtikelCode");
                Map(m => m.DeliveryConditions).Name("DeliveredIn");
            }
        }
    }
}