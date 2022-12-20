using System;
using GemBox.Spreadsheet;
namespace backend_engine.Services
{
	public class PreprocessWorkbook
	{
		public PreprocessWorkbook()
		{

		}

		public static ExcelFile PreProcessFile(ExcelFile workbook) { 
			foreach(ExcelWorksheet sheet in workbook.Worksheets) 

	    {
				IEnumerable<ExcelCell> validCells = workbook.Worksheets[sheet.Name].GetUsedCellRange(false);
				if (validCells != null)
				{

                    foreach (ExcelCell cell in validCells)

                    {
                        if (cell.Value != null && cell.Formula != null && cell.Formula.Contains("ciqfunction"))

                        {
                            cell.Formula = cell.Value.ToString();


                        }

                    }

                }
			

	    
	    
	     }
			return workbook;
		

	
	
	}
	}
}

