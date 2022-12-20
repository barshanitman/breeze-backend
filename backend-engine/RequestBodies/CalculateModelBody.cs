using System;
namespace backend_engine.RequestBodies
{
	public class CalculateModelBody
	{

		public string Model { get; set; }

		public IEnumerable<GenericCell> Inputs{ get; set; }

		public IEnumerable<OutputCell> Outputs { get; set; }


	}
}

