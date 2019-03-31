using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using FileUploader.Domain;
using Serilog;

namespace FileUploader.Storage
{
    public class SqlStorage : IProductStorage
    {
        private readonly ProductsContext _context;
        private readonly int _batchSize;

        public SqlStorage(ProductsContext context, int batchSize = 1000)
        {
            _context = context;
            _batchSize = batchSize;
        }

        public async Task Save(IEnumerable<Product> products, string sessionId)
        {
            await _context.Database.EnsureCreatedAsync();

            if (_context.Products.Any(p => p.SessionId == sessionId))
            {
                throw new ApplicationException("Re-upload and continue upload are not implemented yet. Please start a new upload session.");
            }

            var savedProductsCount = 0;
            var saveBuffer = new List<ProductSqlDto>();

            Log.Debug("Starting saving products stream to the database");
            foreach (var productDto in products.Select(p => Map(p, sessionId)))
            {
                saveBuffer.Add(productDto);

                if (saveBuffer.Count >= _batchSize)
                {
                    await _context.BulkInsertAsync(saveBuffer);
                    savedProductsCount += saveBuffer.Count;
                    saveBuffer.Clear();
                }
            }

            if (saveBuffer.Any())
            {
                await _context.BulkInsertAsync(saveBuffer);
                savedProductsCount += saveBuffer.Count;
            }

            Log.Debug($"Saved {savedProductsCount} products to the database");
        }

        // todo: make unit tested
        private static ProductSqlDto Map(Product model, string sessionId)
        {
            return new ProductSqlDto
            {
                Key = model.Key,
                SessionId = sessionId,
                ArticleCode = model.ArticleCode,
                ColorCode = model.ColorCode,
                Description = model.Description,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                DeliveryConditions = model.DeliveryConditions,
                Q1 = model.Q1,
                Size = model.Size,
                Color = model.Color
            };
        }
    }
}