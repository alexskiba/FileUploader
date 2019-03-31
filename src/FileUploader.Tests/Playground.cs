using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CsvHelper;
using FileUploader.Domain;
using FileUploader.Service;
using FileUploader.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using Shouldly;

namespace FileUploader.Tests
{
    public class Playground
    {
        [SetUp]
        public void Setup()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Console().CreateLogger();
        }

        [Test]
        public async Task JsonLocalStorage()
        {

            var sessionId = Guid.NewGuid().ToString("N");
            var storageFolder = "tmp";
            var expectedFileName = $"{storageFolder}\\{sessionId}.json";
            var jsonStorage = new LocalStorage(storageFolder);

            await jsonStorage.Save(new[]
                                   {
                                       new Product {ArticleCode = "test", Price = 9},
                                       new Product {ArticleCode = "test_2"}
                                   },
                                   sessionId);

            File.Exists(expectedFileName).ShouldBeTrue();
            var stored = JsonConvert.DeserializeObject<Product[]>(File.ReadAllText(expectedFileName));
            stored.Length.ShouldBe(2);
            stored[0].ArticleCode.ShouldBe("test");
            stored[1].ArticleCode.ShouldBe("test_2");
        }

        [Test]
        public async Task SqlStorage()
        {
            Assert.Fail("Specify the connection string!");
            var connectionString = "";
            var sessionId = Guid.NewGuid().ToString("N");
            var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
            optionsBuilder.UseSqlServer(connectionString);
            var dbContext = new ProductsContext(optionsBuilder.Options);
            var sqlStorage = new SqlStorage(dbContext);

            await sqlStorage.Save(new[]
                                  {
                                      new Product {Key = "key1", ArticleCode = "test", Price = 9},
                                      new Product {Key = "key2", ArticleCode = "test_2"}
                                  },
                                  sessionId);

            dbContext.Products.Count(p => p.SessionId == sessionId).ShouldBe(2);
        }

        [Test]
        public void CsvMapper()
        {
            var rawCsv = @"Key,ArtikelCode,ColorCode,Description,Price,DiscountPrice,DeliveredIn,Q1,Size,Color
2800104,2,broek,Gaastra,8,0,1-3 werkdagen,baby,104,grijs
00000002groe56,2,broek,Gaastra,8,0,1-3 werkdagen,baby,56,groen";
            var mapper = new CsvProductMapper();

            var mapped = mapper.MapProducts(new MemoryStream(Encoding.UTF8.GetBytes(rawCsv))).ToList();

            mapped.Count.ShouldBe(2);
            mapped[0].Key.ShouldBe("2800104");
            mapped[0].ArticleCode.ShouldBe("2");
            mapped[0].ColorCode.ShouldBe("broek");
            mapped[0].Description.ShouldBe("Gaastra");
            mapped[0].Price.ShouldBe(8);
            mapped[0].DiscountPrice.ShouldBe(0);
            mapped[0].DeliveryConditions.ShouldBe("1-3 werkdagen");
            mapped[0].Q1.ShouldBe("baby");
            mapped[0].Size.ShouldBe(104);
            mapped[0].Color.ShouldBe("grijs");

            mapped[1].Key.ShouldBe("00000002groe56");
        }

        [TestCase("Sql")]
        [TestCase("Json")]
        public void Autofac(string storageType)
        {
            var builder = new ContainerBuilder();
            var configurationMock = Substitute.For<IConfiguration>();
            configurationMock["ProductStorageType"].Returns(storageType);
            configurationMock["LocalStoragePath"].Returns("not empty string");
            configurationMock.GetConnectionString("ProductsDatabase").Returns("not empty string");
            builder.RegisterModule(new AutofacModule(configurationMock));
            var container = builder.Build();

            var storageService = container.Resolve<StorageService>();

            storageService.ShouldNotBeNull();
        }

        [Test]
        [Ignore("Data generator")]
        public void GenerateBigFile()
        {
            var multiplicationFactor = 2000;
            var src = @"..\..\..\..\..\Data\iru-assignment-2018_sample.csv";
            var dst = @"..\..\..\..\..\Data\iru-assignment-2018_big.csv";

            using (var reader = new StreamReader(src))
            {
                using (var csvReader = new CsvReader(reader))
                {
                    using (var writer = new StreamWriter(dst))
                    {
                        using (var csvWriter = new CsvWriter(writer))
                        {
                            csvReader.Read();
                            csvReader.ReadHeader();
                            var headers = csvReader.Context.HeaderRecord;
                            foreach (var header in headers)
                            {
                                csvWriter.WriteField(header);
                            }

                            csvWriter.NextRecord();

                            while (csvReader.Read())
                            {
                                for (var i = 0; i < multiplicationFactor; i++)
                                {
                                    foreach (var header in headers)
                                    {
                                        var value = csvReader.GetField(header);
                                        if (header == "Key")
                                        {
                                            value = Guid.NewGuid().ToString("N");
                                        }

                                        csvWriter.WriteField(value);
                                    }

                                    csvWriter.NextRecord();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}