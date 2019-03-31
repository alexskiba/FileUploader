using Microsoft.EntityFrameworkCore;

namespace FileUploader.Storage
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        public DbSet<ProductSqlDto> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductSqlDto>().HasKey(p => new {p.Key, p.SessionId});
        }
    }

    public class ProductSqlDto
    {
        public string Key { get; set; }
        public string SessionId { get; set; }
        public string ArticleCode { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public string DeliveryConditions { get; set; }
        public string Q1 { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
    }
}