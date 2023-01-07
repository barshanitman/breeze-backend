using System;
namespace backend_engine.Models
{
	public class SummaryTearSheetOutput:IBaseEntity
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string SheetReference { get; set; }

		public string CellReference { get; set; }

	
	}
}

