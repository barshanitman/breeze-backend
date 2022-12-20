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
    public class StockController:ControllerBase
	{

		private readonly IRepository<Stock> _repo;

		private readonly ILogger<ModelEngineController> _logger;

		public StockController(ILogger<ModelEngineController> logger, IRepository<Stock> repo)
		{

			_logger = logger;
			_repo = repo;


		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Stock>>> GetAll()
	{
	    return Ok(await _repo.GetAll());


	}

	[HttpGet]
    [Route("/Stock/{id}")]
		public async Task<ActionResult<IEnumerable<Stock>>> GetById(int id)
	{
	    return Ok(await _repo.GetById(id));


	}

	[HttpPost]
    public async Task<Stock> AddEntity([FromBody] Stock entity)

	   {

	    Stock res = _repo.Add(entity);
		await _repo.SaveChanges();
	    return res;

		}

		[HttpDelete]
		[Route("/Stock/{id}")]
		public async Task<ActionResult> RemoveEntity(int id)

		{
			await _repo.RemoveEntity(id);
			return Ok();



		}



	}



}

