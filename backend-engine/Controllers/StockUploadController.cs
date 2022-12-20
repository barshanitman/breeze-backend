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
    public class StockUploadController:ControllerBase
	{

		private readonly IRepository<StockUpload> _repo;

		private readonly ILogger<ModelEngineController> _logger;

		public StockUploadController(ILogger<ModelEngineController> logger, IRepository<StockUpload> repo)
		{

			_logger = logger;
			_repo = repo;


		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<StockUpload>>> GetAllStocks()
	{
	    return Ok(await _repo.GetAll());

	}

	[HttpGet]
    [Route("/StockUpload/{id}")]
		public async Task<ActionResult<IEnumerable<StockUpload>>> GetAllStocks(int id)
	{
	    return Ok(await _repo.GetById(id));

	}

	[HttpPost]
    public async Task<StockUpload> AddStock([FromBody] StockUpload entity)

	   {

	    StockUpload res =_repo.Add(entity);
		await _repo.SaveChanges();
	    return res;

		}
		[HttpDelete]
		[Route("/StockUpload/{id}")]
		public async Task<ActionResult> RemoveEntity(int id)

		{
			await _repo.RemoveEntity(id);
			return Ok();



		}


       
    }
}

