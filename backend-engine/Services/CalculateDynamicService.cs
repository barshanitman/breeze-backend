using System;
using System.Collections.Generic;
using backend_engine.Models;
using GemBox.Spreadsheet;

namespace backend_engine.Services
{
    public class CalculateDynamicService
    {



        public CalculateDynamicService()
        {

        }

        public static List<KeyValuePair<string, object>> CalculateFinancialYearTearSheet(List<TearSheetOutput> tearSheetMapping, ExcelFile workbook)
        {

            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();

            foreach (TearSheetOutput map in tearSheetMapping)

            {
                workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Calculate();
                results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));

            }

            return results;



        }

        public static List<KeyValuePair<string, object>> GetOriginalTearSheetData(List<TearSheetOutput> tearsheetMappings,

            ExcelFile workbook)
        {

            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();
            foreach (TearSheetOutput map in tearsheetMappings)
            {

                results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));


            }

            return results;

        }

        public static List<KeyValuePair<string, object>> CalculateDriverTearSheet(
            List<DriverTearSheetOutput> driverSheetMappings, ExcelFile workbook
            )
        {
            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();

            foreach (DriverTearSheetOutput map in driverSheetMappings)
            {
                if (map.IsFormula)
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));

                }
                else
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, 0));

                }
            }

            return results;


        }






    }
}

