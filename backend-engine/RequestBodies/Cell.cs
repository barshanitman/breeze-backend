using System;
namespace backend_engine.RequestBodies
{
	public class GenericCell
	{

		public string sheet { get; set; }
		public string  reference { get; set; }
		public string value { get; set; }

	}

	public class OutputCell 
    
    { 
		public string sheet { get; set; }
		public string  reference { get; set; }

    
    }

	
}

