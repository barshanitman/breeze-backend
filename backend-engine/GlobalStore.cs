using System;
using GemBox.Spreadsheet;
namespace backend_engine
{
	public static class GlobalStore
	{
		public static Dictionary<string, ExcelFile> workbooks;
		static GlobalStore()
		{
			 workbooks = new Dictionary<string, ExcelFile>();
		}



	}
}

