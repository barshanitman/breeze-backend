using System;
using Microsoft.AspNetCore.Mvc;
using GemBox.Spreadsheet;
using backend_engine.RequestBodies;
using Microsoft.Extensions.Caching.Memory;
using Azure.Storage.Blobs;
using backend_engine.Services;
using MongoDB.Driver;
using backend_engine.Models;
using backend_engine.ResponseBodies;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace backend_engine.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ModelEngineController : ControllerBase
	{

		private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.net";
		private const string containerName = "hermes-workbooks";
		private readonly ILogger<ModelEngineController> _logger;
		private readonly IMemoryCache _memoryCache;


		public ModelEngineController(ILogger<ModelEngineController> logger, IMemoryCache memoryCache)
		{
			_logger = logger;
			_memoryCache = memoryCache;

		}

		[HttpPost]
		public Object CalculateModel(CalculateModelBody request)
		{
			SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");

			ExcelFile workbook = _memoryCache.Get<ExcelFile>(request.Model.ToString());

			if (workbook is null)
			{
				BlobClient blobClient = new BlobClient(connectionString, containerName, request.Model.ToString());
				var stream = new MemoryStream();
				blobClient.DownloadTo(stream);
				workbook = ExcelFile.Load(stream);
				workbook = PreprocessWorkbook.PreProcessFile(workbook);
				_memoryCache.Set(request.Model.ToString(), workbook, TimeSpan.FromHours(24));
				stream.Close();

			}




			foreach (GenericCell cell in request.Inputs)
			{


				workbook.Worksheets[cell.sheet.ToString()].Cells[cell.reference.ToString()].Formula = cell.value;


			}


			List<GenericCell> results = new List<GenericCell>();

			foreach (OutputCell cell in request.Outputs)
			{


				workbook.Worksheets[cell.sheet.ToString()].Cells[cell.reference.ToString()].Calculate();
				GenericCell outputCell = new GenericCell();
				outputCell.reference = cell.reference;
				outputCell.sheet = cell.sheet;
				outputCell.value = workbook.Worksheets[cell.sheet.ToString()].Cells[cell.reference.ToString()].Value.ToString();
				results.Add(outputCell);

			}


			return results;



		}
		[HttpGet]
		public object GetModels()
		{

			BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
			var blobs = container.GetBlobs();
			return blobs;

		}

		


	}
}

