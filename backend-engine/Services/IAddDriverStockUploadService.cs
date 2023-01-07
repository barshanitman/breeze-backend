using System;
using GemBox.Spreadsheet;

namespace backend_engine.Services
{
	public interface IAddDriverStockUploadService
	{
		public void AddDriversToStockUpload(ExcelFile workbook,int stockUploadId, string sheetName);

        public string ExtractSheetName(string cell);

        public string ExtractCellReference(string formula);



    }
}

