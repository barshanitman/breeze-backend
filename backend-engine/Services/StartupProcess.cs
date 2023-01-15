using System;
using backend_engine.Models;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Caching.Memory;


using Azure.Storage.Blobs;

namespace backend_engine.Services
{
    public class StartupProcess : IHostedService
    {


        private readonly IServiceProvider _serviceProvider;
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.net";
        private const string containerName = "hermes-workbooks";



        public StartupProcess(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");
            using (var scope = _serviceProvider.CreateScope())
            {

                var _context = scope.ServiceProvider.GetService<BreezeDataContext>();
                var _cache = scope.ServiceProvider.GetService<IMemoryCache>();


                IEnumerable<TearSheetOutput> tearsheetMappings = _context.TearSheetOutputs.ToList();

                IEnumerable<SummaryTearSheetOutput> summarySheetMappings = _context.SummaryTearSheetOutputs.ToList();

                IEnumerable<DriverTearSheetOutput> driverSheetMappings = _context.DriverTearSheetOutputs.ToList();


                _cache.Set("TearSheetMappings", tearsheetMappings);
                _cache.Set("SummarySheetMappings", summarySheetMappings);


                BlobServiceClient blobClientConnection = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobClientConnection.GetBlobContainerClient(containerName);
                var blobs = containerClient.GetBlobs();

                foreach (var blob in blobs)
                {
                    BlobClient blobClient = containerClient.GetBlobClient(blob.Name);
                    var stream = new MemoryStream();
                    blobClient.DownloadTo(stream);
                    ExcelFile workbook = ExcelFile.Load(stream);
                    workbook = PreprocessWorkbook.PreProcessFile(workbook);
                    workbook.CalculationOptions.EnableIterativeCalculation = true;
                    _cache.Set(blob.Name, workbook);
                    stream.Close();
                    Console.WriteLine($"Finished loading workook:{blob.Name}");

                }


            }

            return Task.CompletedTask;


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

