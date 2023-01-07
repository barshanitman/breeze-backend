using System;
using Azure.Storage.Blobs;
using backend_engine.Models;
using backend_engine.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using backend_engine.RequestBodies;
using backend_engine.Repositories;
using Microsoft.Extensions.Caching.Memory;
using GemBox.Spreadsheet;

namespace backend_engine.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ModelUploadController:ControllerBase
	{
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.net";
        private const string containerName = "hermes-workbooks";
		private readonly IRepository<StockUpload> _stockUploadRepo;
		private readonly IRepository<TearSheetOutput> _tearSheetOutputRepo;
		private readonly IRepository<StockUploadTearsheetValue> _stockUploadTearSheetValueRepo;

		private readonly IMemoryCache _memoryCache;

        private readonly BreezeDataContext _context;
		private readonly ILogger<ModelUploadController> _logger;

		private readonly IAddDriverStockUploadService _addDriverStockUploadService;


	public ModelUploadController(ILogger<ModelUploadController> logger, IRepository<StockUpload> stockUploadRepo, 
		IAddDriverStockUploadService addDriverStockUploadService,
	IMemoryCache memoryCache, IRepository<TearSheetOutput> tearSheetOutputRepo, IRepository<StockUploadTearsheetValue>stockUploadTearSheetValueRepo, BreezeDataContext context)

		{
			_stockUploadRepo = stockUploadRepo;
			_memoryCache = memoryCache;
			_logger = logger;
			_tearSheetOutputRepo = tearSheetOutputRepo;
			_stockUploadTearSheetValueRepo = stockUploadTearSheetValueRepo;
			_context = context;
			_addDriverStockUploadService = addDriverStockUploadService;

		}


		[HttpPost]
		[Route("upload")]
		public async Task<IActionResult> Upload(IFormFile file, IFormCollection data) 
	{
			var uniqueFileName =  data["uniqueFileName"].ToString();


			BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

			SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");

			//Creating unique workbook name
			//string fileName = Guid.NewGuid() +  file.FileName;
			using(var stream = new MemoryStream()) 
	    {

				file.CopyTo(stream);
				ExcelFile workbook = ExcelFile.Load(stream);
                workbook = PreprocessWorkbook.PreProcessFile(workbook);
                _memoryCache.Set(uniqueFileName, workbook, TimeSpan.FromHours(24));
				stream.Position = 0;
				blobContainerClient.UploadBlob(uniqueFileName,stream);
				stream.Close();
	    
	    }

			ExcelFile retrievedWorkbook = _memoryCache.Get<ExcelFile>(uniqueFileName);
			StockUpload retrievedStockUpload = _context.StockUploads.Where(x => x.FileName == uniqueFileName).First();
		
			_addDriverStockUploadService.AddDriversToStockUpload(retrievedWorkbook,retrievedStockUpload.Id, "Tearsheet template");

			return Ok();

	    }



	}



	}

       

