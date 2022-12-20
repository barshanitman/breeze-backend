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
    public class TearSheetOutputController:ControllerBase
	{

		private readonly IRepository<TearSheetOutput> _repo;

		private readonly ILogger<TearSheetOutput> _logger;

		public TearSheetOutputController(ILogger<TearSheetOutput> logger, IRepository<TearSheetOutput> repo)
		{

			_logger = logger;
			_repo = repo;


		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TearSheetOutput>>> GetAll()
	{
	    return Ok(await _repo.GetAll());

	}

	[HttpGet]
    [Route("/TearSheetOutput/{id}")]
		public async Task<ActionResult<IEnumerable<TearSheetOutput>>> GetById(int id)
	{
	    return Ok(await _repo.GetById(id));

	}

	[HttpPost]
    public async Task<TearSheetOutput> AddEntity([FromBody] TearSheetOutput entity)

	   {

	    TearSheetOutput res =_repo.Add(entity);
		await _repo.SaveChanges();
	    return res;

		}

		[HttpDelete]
		[Route("/TearSheetOutput/{id}")]
		public async Task<ActionResult> RemoveEntity(int id)

		{
			await _repo.RemoveEntity(id);
			return Ok();



		}


       
    }
}

