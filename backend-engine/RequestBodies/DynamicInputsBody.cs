using System;
namespace backend_engine.RequestBodies
{
	public class DynamicInputsBody
	{
		public int workbookId { get; set; }
		//public IEnumerable<GenericCell> cellInputs { get; set; }
		public IEnumerable<TearSheetReference> cellInputs {get;set;}
		public IEnumerable<DriverReference>  driverInputs {get;set;}


	}

	public class TearSheetReference
	{
		public string name { get; set; }

		public int financialYearId { get; set; }

		public object value { get; set; }
    
    }

	public class DriverReference
	{ 

		public string name { get; set; }

		public int financialYearId { get; set; }

		public object value { get; set; }
    
    
    
    }
}

