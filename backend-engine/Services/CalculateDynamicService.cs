using System;
using System.Collections.Generic;
using backend_engine.Models;
using GemBox.Spreadsheet;
using backend_engine.RequestBodies;


namespace backend_engine.Services
{
    public class CalculateDynamicService
    {


        public CalculateDynamicService()
        {

        }

        public static List<KeyValuePair<string, object>> CalculateFinancialYearTearSheet(List<TearSheetOutput> tearSheetMapping, ExcelFile workbook, int financialYearId)
        {

            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();






            //Fix Gross Margin

            // workbook.Worksheets["Tearsheet template"].Cells["B21"].Formula = workbook.Worksheets["Tearsheet template"].Cells["B21"].Value.ToString();
            // workbook.Worksheets["Tearsheet template"].Cells["C21"].Formula = workbook.Worksheets["Tearsheet template"].Cells["C21"].Value.ToString();
            // workbook.Worksheets["Tearsheet template"].Cells["D21"].Formula = workbook.Worksheets["Tearsheet template"].Cells["D21"].Value.ToString();
            // workbook.Worksheets["Tearsheet template"].Cells["E21"].Formula = workbook.Worksheets["Tearsheet template"].Cells["E21"].Value.ToString();
            // workbook.Worksheets["Tearsheet template"].Cells["F21"].Formula = workbook.Worksheets["Tearsheet template"].Cells["F21"].Value.ToString();




            foreach (TearSheetOutput map in tearSheetMapping)

            {

                workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Calculate();
                results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));


            }

            //custom logic for Gross Profit
            object grossProfitResult;

            if (financialYearId == 1)
            {

                workbook.Worksheets["Tearsheet template"].Cells["B21"].Formula = "=B64 * B19";
                workbook.Worksheets["Tearsheet template"].Cells["B21"].Calculate();
                grossProfitResult = workbook.Worksheets["Tearsheet template"].Cells["B21"].Value;

            }
            else if (financialYearId == 2)
            {
                workbook.Worksheets["Tearsheet template"].Cells["C21"].Formula = "=C64 * C19";
                workbook.Worksheets["Tearsheet template"].Cells["C21"].Calculate();
                grossProfitResult = workbook.Worksheets["Tearsheet template"].Cells["C21"].Value;

            }

            else if (financialYearId == 3)
            {
                workbook.Worksheets["Tearsheet template"].Cells["D21"].Formula = "=D64 * D19";
                workbook.Worksheets["Tearsheet template"].Cells["D21"].Calculate();
                grossProfitResult = workbook.Worksheets["Tearsheet template"].Cells["D21"].Value;

            }

            else if (financialYearId == 4)
            {
                workbook.Worksheets["Tearsheet template"].Cells["E21"].Formula = "=E64 * E19";
                workbook.Worksheets["Tearsheet template"].Cells["E21"].Calculate();
                grossProfitResult = workbook.Worksheets["Tearsheet template"].Cells["E21"].Value;

            }

            else
            {

                workbook.Worksheets["Tearsheet template"].Cells["F21"].Formula = "=F64 * F19";
                workbook.Worksheets["Tearsheet template"].Cells["F21"].Calculate();
                grossProfitResult = workbook.Worksheets["Tearsheet template"].Cells["F21"].Value;


            }
            KeyValuePair<string, object> grossProfitKeyValuePair = new KeyValuePair<string, object>("Gross Profit", grossProfitResult);




            results[results.FindIndex(x => x.Key == "Gross Profit")] = grossProfitKeyValuePair;





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

        public static List<KeyValuePair<string, object>> CalculateDynamicDriverTearSheet(
            List<DriverTearSheetOutput> driverSheetMappings, ExcelFile workbook


            )
        {
            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();
            foreach (DriverTearSheetOutput map in driverSheetMappings)
            {

                if (map.IsFormula)
                {

                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                }
                else
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, 0));

                }



            }
            return results;

        }

        public static ExcelFile UpdateDriverInputs(IEnumerable<DriverReference> driverInputs, ExcelFile workbook, IEnumerable<DriverTearSheetOutput> financialYearDriverMappings)
        {

            IEnumerable<DriverTearSheetOutput> firstYearActualLookup = financialYearDriverMappings.Where(x => x.FinancialYearId == 1);
            IEnumerable<DriverTearSheetOutput> secondYearActualLookup = financialYearDriverMappings.Where(x => x.FinancialYearId == 2);
            IEnumerable<DriverTearSheetOutput> thirdYearForecastLookup = financialYearDriverMappings.Where(x => x.FinancialYearId == 3);
            IEnumerable<DriverTearSheetOutput> fourthYearForecastLookup = financialYearDriverMappings.Where(x => x.FinancialYearId == 4);
            IEnumerable<DriverTearSheetOutput> fifthYearForecastLookup = financialYearDriverMappings.Where(x => x.FinancialYearId == 5);


            foreach (DriverReference driverInput in driverInputs)

            {
                if (driverInput.financialYearId == 1)
                {

                    DriverTearSheetOutput cellLocation = firstYearActualLookup.Where(x => x.Name == driverInput.name).First();
                    workbook.Worksheets[cellLocation.SheetReference].Cells[cellLocation.CellReference].Formula = driverInput.value.ToString();


                }


                else if (driverInput.financialYearId == 2)
                {


                    DriverTearSheetOutput cellLocation = secondYearActualLookup.Where(x => x.Name == driverInput.name).First();
                    workbook.Worksheets[cellLocation.SheetReference].Cells[cellLocation.CellReference].Formula = driverInput.value.ToString();


                }


                else if (driverInput.financialYearId == 3)
                {

                    DriverTearSheetOutput cellLocation = thirdYearForecastLookup.Where(x => x.Name == driverInput.name).First();
                    workbook.Worksheets[cellLocation.SheetReference].Cells[cellLocation.CellReference].Formula = driverInput.value.ToString();


                }


                else if (driverInput.financialYearId == 4)
                {

                    DriverTearSheetOutput cellLocation = fourthYearForecastLookup.Where(x => x.Name == driverInput.name).First();
                    workbook.Worksheets[cellLocation.SheetReference].Cells[cellLocation.CellReference].Formula = driverInput.value.ToString();


                }

                else if (driverInput.financialYearId == 5)

                {

                    DriverTearSheetOutput cellLocation = fifthYearForecastLookup.Where(x => x.Name == driverInput.name).First();
                    workbook.Worksheets[cellLocation.SheetReference].Cells[cellLocation.CellReference].Formula = driverInput.value.ToString();


                }




            }


            return workbook;

        }
        public static ExcelFile UpdateTearSheetInputs(IEnumerable<TearSheetReference> cellInputs, ExcelFile workbook, List<TearSheetOutput> allTearsheetOutputs)
        {

            foreach (TearSheetReference inputCell in cellInputs)

            {
                TearSheetOutput CellLocationOfItem = allTearsheetOutputs.Where(x => x.Name == inputCell.name).First();
                workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();



            }

            return workbook;

        }
    }


}

