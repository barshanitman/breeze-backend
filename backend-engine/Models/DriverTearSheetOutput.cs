using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models
{
	public class DriverTearSheetOutput:IBaseEntity
	{
		


		public int Id { get; set; }

		[ForeignKey(nameof(StockUploadId))]
		public int StockUploadId {get;set;}

		public string Name { get; set; }

		[ForeignKey(nameof(FinancialYearId))]
		public int FinancialYearId { get; set; }

		public string SheetReference { get; set; }

		public string CellReference { get; set; }

		public bool IsFormula { get; set; }


    }
}

