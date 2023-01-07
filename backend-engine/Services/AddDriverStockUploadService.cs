using System;
using backend_engine.Repositories;
using backend_engine.Models;
using GemBox.Spreadsheet;
using System.Text.RegularExpressions;

namespace backend_engine.Services
{
	public class AddDriverStockUploadService:IAddDriverStockUploadService
	{
		private readonly IRepository<DriverTearSheetOutput> _contextDriverTearSheetOutput;

        private readonly BreezeDataContext _context;
		public AddDriverStockUploadService(IRepository<DriverTearSheetOutput> driverTearSheetOutputRepo, BreezeDataContext context)
		{
			_contextDriverTearSheetOutput = driverTearSheetOutputRepo;
            _context = context;

		}

		public async void AddDriversToStockUpload(ExcelFile workbook,int stockUploadId, string sheetName) {
            int rowIndex;

            for (rowIndex = 18; rowIndex <= 74; rowIndex++)
            {


                if (workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].ValueType.ToString() == "Null")
                {
                    break;

                }

                List<DriverTearSheetOutput> collection = new List<DriverTearSheetOutput>();

                if (workbook.Worksheets[sheetName].Cells[$"I{rowIndex}"].ValueType.ToString() != "Null")
                {
                    DriverTearSheetOutput driverFirstYearActual = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 1,
                        SheetReference = ExtractSheetName(workbook.Worksheets[sheetName].Cells[$"I{rowIndex}"].Formula),
                        CellReference = ExtractCellReference(workbook.Worksheets[sheetName].Cells[$"I{rowIndex}"].Formula),
                        StockUploadId = stockUploadId,
                        IsFormula = true


                    };

                    collection.Add(driverFirstYearActual);

                }

                else
                {

                    DriverTearSheetOutput driverFirstYearActual = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 1,
                        SheetReference ="None" ,
                        CellReference = "None" ,
                        StockUploadId = stockUploadId,
                        IsFormula = false


                    };


                    collection.Add(driverFirstYearActual);
                }



                if (workbook.Worksheets[sheetName].Cells[$"J{rowIndex}"].ValueType.ToString() != "Null")
                {

                    DriverTearSheetOutput driverSecondYearActual = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 2,
                        SheetReference = ExtractSheetName(workbook.Worksheets[sheetName].Cells[$"J{rowIndex}"].Formula),
                        CellReference = ExtractCellReference(workbook.Worksheets[sheetName].Cells[$"J{rowIndex}"].Formula),
                        StockUploadId = stockUploadId,
                        IsFormula = true


                    };

                    collection.Add(driverSecondYearActual);

                }

                else
                {

                    DriverTearSheetOutput driverSecondYearActual = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 2,
                        SheetReference = "None" ,
                        CellReference = "None" ,
                        StockUploadId = stockUploadId,
                        IsFormula = false


                    };

                    collection.Add(driverSecondYearActual);


                }

                if (workbook.Worksheets[sheetName].Cells[$"K{rowIndex}"].ValueType.ToString() != "Null")
                {


                    DriverTearSheetOutput driverThirdYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 3,
                        SheetReference = ExtractSheetName(workbook.Worksheets[sheetName].Cells[$"K{rowIndex}"].Formula),
                        CellReference = ExtractCellReference(workbook.Worksheets[sheetName].Cells[$"K{rowIndex}"].Formula),
                        StockUploadId = stockUploadId,
                        IsFormula = true

                    };


                    collection.Add(driverThirdYearForecast);

                }

                else
                {


                    DriverTearSheetOutput driverThirdYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 3,
                        SheetReference = "None" ,
                        CellReference = "None" ,
                        StockUploadId = stockUploadId,
                        IsFormula = false

                    };


                    collection.Add(driverThirdYearForecast);



                }
                if (workbook.Worksheets[sheetName].Cells[$"L{rowIndex}"].ValueType.ToString() != "Null")
                {

                    DriverTearSheetOutput driverFourthYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 4,
                        SheetReference = ExtractSheetName(workbook.Worksheets[sheetName].Cells[$"L{rowIndex}"].Formula),
                        CellReference = ExtractCellReference(workbook.Worksheets[sheetName].Cells[$"L{rowIndex}"].Formula),
                        StockUploadId = stockUploadId,
                        IsFormula = true


                    };


                    collection.Add(driverFourthYearForecast);

                }

                else
                {

                    DriverTearSheetOutput driverFourthYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 4,
                        SheetReference = "None",
                        CellReference = "None",
                        StockUploadId = stockUploadId,
                        IsFormula = false


                    };


                    collection.Add(driverFourthYearForecast);



                }



                if (workbook.Worksheets[sheetName].Cells[$"M{rowIndex}"].ValueType.ToString() != "Null")
                {

                    DriverTearSheetOutput driverFifthYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 5,
                        SheetReference = ExtractSheetName(workbook.Worksheets[sheetName].Cells[$"M{rowIndex}"].Formula),
                        CellReference = ExtractCellReference(workbook.Worksheets[sheetName].Cells[$"M{rowIndex}"].Formula),
                        StockUploadId = stockUploadId,
                        IsFormula = true


                    };


                    collection.Add(driverFifthYearForecast);

                }
                else
                {

                    DriverTearSheetOutput driverFifthYearForecast = new DriverTearSheetOutput

                    {
                        Name = workbook.Worksheets[sheetName].Cells[$"H{rowIndex}"].Value.ToString(),
                        FinancialYearId = 5,
                        SheetReference = "None" ,
                        CellReference = "None" ,
                        StockUploadId = stockUploadId,
                        IsFormula = false


                    };


                    collection.Add(driverFifthYearForecast);





                }


                _context.DriverTearSheetOutputs.AddRange(collection);
                _context.SaveChanges();

            }


	
	
	}
        public string ExtractSheetName(string formula)
        {
            string cell = ExtractCellReference(formula);
            string sheetName = formula.Replace(cell, "").Replace("'", "").Replace("!", "").Replace("=", "");

            if (sheetName == "")
            {

                return "Tearsheet Template";

            }

            else
            {

                return sheetName;

            }


        }

        public string ExtractCellReference(string formula) {

      
          


            string regex = @"\$?[A-Z]{1,3}\$?[0-9]{1,7}";
            Match cellReference = Regex.Match(formula, regex);
            return cellReference.ToString();



        }


    }
}

