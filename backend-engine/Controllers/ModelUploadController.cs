using System;
using Azure.Storage.Blobs;
using backend_engine.Models;
using backend_engine.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using backend_engine.RequestBodies;
using backend_engine.Repositories;

namespace backend_engine.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ModelUploadController:ControllerBase
	{
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.net";
        private const string containerName = "hermes-workbooks";

	private readonly IRepository<Stock> _stockRepo;
	private readonly IRepository<StockUpload> _stockUploadRepo;
    private readonly ILogger<ModelUploadController> _logger;

	public ModelUploadController(ILogger<ModelUploadController> logger, IRepository<Stock> stockRepo, IRepository<StockUpload>stockUploadRepo)

		{

			_logger = logger;
		    _stockRepo = stockRepo;
		    _stockUploadRepo = stockUploadRepo;


		}

		[HttpPost]
		[Route("upload")]
		public async Task<IActionResult> Upload(IFormFile file) 
	{
			BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

			string fileName = Guid.NewGuid().ToString() +  file.FileName;
			using(var stream = new MemoryStream()) 
	    {
				file.CopyTo(stream);
				stream.Position = 0;
				blobContainerClient.UploadBlob(fileName,stream);
				stream.Close();
	    
	    }
			Stock newStock = new Stock();
		    newStock.Name = fileName.Remove(fileName.Length - 5);
			newStock.Code = newStock.Name;
			Stock res = _stockRepo.Add(newStock);
			await _stockRepo.SaveChanges();
			StockUpload newStockUpload = new StockUpload();
			newStockUpload.FileName = fileName;
			newStockUpload.StockId = (int)res.Id;

			StockUpload resStockUpload = _stockUploadRepo.Add(newStockUpload);
			await _stockUploadRepo.SaveChanges();
            
            return Ok(res);


	}

       
    }
}

