using System.Collections.Generic;
using System.IO;
using CsvHelper;
using FileUploader.Domain;

namespace FileUploader.Service
{
    public class CsvProductMapper : IProductMapper
    {
        public IEnumerable<Product> MapProducts(Stream inputStream)
        {
            using (var reader = new StreamReader(inputStream))
            {
                using (var csvReader = new CsvReader(reader))
                {
                    while (csvReader.Read())
                    {
                        yield return csvReader.GetRecord<Product>();
                    }
                }
            }
        }
    }
}