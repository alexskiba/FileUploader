# FileUploader

Receives and stores potentially very large file as a set of records.

### Configuration
**Specify a connection string `ProductsDatabase` before running with Sql storage type!**

`ProductStorageType` - "Sql" or "Json", defines how uploaded records will be stored.

`LocalStoragePath` - used with "Json" storage type, specifies a folder where uploaded records will be stored.

`Serilog` - standard Serilog logging configuration.

### Testing
To build and start the service run [run.bat](/run.bat)

To upload a file ([sample](/data/iru-assignment-2018_sample.csv)) run `curl -X POST -T iru-assignment-2018_sample.csv https://localhost:44359/api/upload -H "Transfer-Encoding: chunked" -H "Content-Length:"`

To test with a large file:
 - Comment out the `Ignore` attribute on `GenerateBigFile` test at the [Playground](src/FileUploader.Tests/Playground.cs).
 - Run `GenerateBigFile` test.
 - Run `curl -X POST -T iru-assignment-2018_big.csv https://localhost:44359/api/upload -H "Transfer-Encoding: chunked" -H "Content-Length:"`
 - You can adjust the size of a generated large file by changing the hardcoded `multiplicationFactor` value.