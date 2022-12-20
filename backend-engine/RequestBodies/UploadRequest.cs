using System;
namespace backend_engine.RequestBodies
{
	public class UploadRequest
	{
		public IFormFile file { get; set; }

		public string name { get; set; }

		public string code { get; set; }
	}
}

