using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models
{
	public class StockUploadTearsheetValue:IBaseEntity
	{
		public int Id { get; set; }

		[ForeignKey(nameof(TearSheetOutputId))]
		public int TearSheetOutputId {get;set;}

		[ForeignKey(nameof(StockUploadId))]
		public int StockUploadId {get;set;}
		
		public double Value {get;set;}

	}
    
}

