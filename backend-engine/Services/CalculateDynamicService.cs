using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using backend_engine.Models;
using Microsoft.Extensions.Caching.Memory;
using Azure.Storage.Blobs;
using backend_engine.Services;
using backend_engine.RequestBodies;
using backend_engine.Repositories;



namespace backend_engine.Services
{
    public class CalculateDynamicService
    {

        public CalculateDynamicService()
        {


        }

        public static List<KeyValuePair<string, object>> CalculateFinancialYearTearSheet(List<TearSheetOutput> tearSheetMapping, ExcelFile workbook, int financialYearId, bool grossMarginIsInput)
        {

            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();

            if (grossMarginIsInput)
            {


                workbook.Worksheets["Tearsheet template"].Cells["B64"].Calculate();
                workbook.Worksheets["Tearsheet template"].Cells["C64"].Calculate();
                workbook.Worksheets["Tearsheet template"].Cells["D64"].Calculate();
                workbook.Worksheets["Tearsheet template"].Cells["E64"].Calculate();
                workbook.Worksheets["Tearsheet template"].Cells["F64"].Calculate();


            }
            else
            {


                workbook.Worksheets["Tearsheet template"].Cells["B64"].Formula = workbook.Worksheets["Tearsheet template"].Cells["B64"].Value.ToString();
                workbook.Worksheets["Tearsheet template"].Cells["B64"].Calculate();

                workbook.Worksheets["Tearsheet template"].Cells["C64"].Formula = workbook.Worksheets["Tearsheet template"].Cells["C64"].Value.ToString();
                workbook.Worksheets["Tearsheet template"].Cells["C64"].Calculate();

                workbook.Worksheets["Tearsheet template"].Cells["D64"].Formula = workbook.Worksheets["Tearsheet template"].Cells["D64"].Value.ToString();
                workbook.Worksheets["Tearsheet template"].Cells["D64"].Calculate();

                workbook.Worksheets["Tearsheet template"].Cells["E64"].Formula = workbook.Worksheets["Tearsheet template"].Cells["E64"].Value.ToString();
                workbook.Worksheets["Tearsheet template"].Cells["E64"].Calculate();

                workbook.Worksheets["Tearsheet template"].Cells["F64"].Formula = workbook.Worksheets["Tearsheet template"].Cells["F64"].Value.ToString();
                workbook.Worksheets["Tearsheet template"].Cells["F64"].Calculate();




            }







            // foreach (TearSheetOutput map in tearSheetMapping)

            // {

            //     workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Calculate();
            //     // results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));


            // }

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
            foreach (TearSheetOutput map in tearSheetMapping)

            {

                workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Calculate();

                if (workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value == null)
                {


                    results.Add(new KeyValuePair<string, object>(map.Name, 0));

                }
                else
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));

                }


            }
            return results;






            // results[results.FindIndex(x => x.Key == "Gross Profit")] = grossProfitKeyValuePair;






        }

        public static List<KeyValuePair<string, object>> GetOriginalTearSheetData(List<TearSheetOutput> tearsheetMappings,

            ExcelFile workbook)
        {

            List<KeyValuePair<string, object>> results = new List<KeyValuePair<string, object>>();
            foreach (TearSheetOutput map in tearsheetMappings)
            {

                if (workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value == null)
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, 0));

                }
                else
                {

                    results.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference].Cells[map.CellReference].Value));


                }




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

        public static async Task<object> CalculateTearSheet(ExcelFile workbook, List<SummaryTearSheetOutput> summaryTearSheetOutputs, StockUpload stockUpload, List<TearSheetOutput> allTearsheetOutputs)
        {

            List<KeyValuePair<string, object>> resultsSummaryTearSheetValue = new List<KeyValuePair<string, object>>();

            foreach (SummaryTearSheetOutput map in summaryTearSheetOutputs)

            {

                resultsSummaryTearSheetValue.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

            }

            List<DriverTearSheetOutput> driverMappingFirstYearActual = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 1).ToList();
            List<DriverTearSheetOutput> driverMappingSecondYearActual = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 2).ToList();
            List<DriverTearSheetOutput> driverMappingThirdYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 3).ToList();
            List<DriverTearSheetOutput> driverMappingFourthYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 4).ToList();
            List<DriverTearSheetOutput> driverMappingFifthYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 5).ToList();

            List<KeyValuePair<string, object>> driverResultsFirstYearActual = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsSecondYearActual = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsThirdYearForecast = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsFourthYearForecast = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsFifthYearForecast = new List<KeyValuePair<string, object>>();


            Thread driverFirstYearActualThread = new Thread(() =>
            {
                driverResultsFirstYearActual = CalculateDynamicService.CalculateDriverTearSheet(driverMappingFirstYearActual, workbook);


            });


            Thread driverSecondYearActualThread = new Thread(() =>
            {
                driverResultsSecondYearActual = CalculateDynamicService.CalculateDriverTearSheet(driverMappingSecondYearActual, workbook);

            });


            Thread driverThirdYearForecastThread = new Thread(() =>
                           {
                               driverResultsThirdYearForecast = CalculateDynamicService.CalculateDriverTearSheet(driverMappingThirdYearForecast, workbook);


                           });

            Thread driverFourthYearForecastThread = new Thread(() =>
                                          {
                                              driverResultsFourthYearForecast = CalculateDynamicService.CalculateDriverTearSheet(driverMappingFourthYearForecast, workbook);


                                          });


            Thread driverFifthYearForecastThread = new Thread(() =>
                                                      {


                                                          driverResultsFifthYearForecast = CalculateDynamicService.CalculateDriverTearSheet(driverMappingFifthYearForecast, workbook);
                                                      });


            driverFirstYearActualThread.Start();
            driverFirstYearActualThread.Join();

            driverSecondYearActualThread.Start();
            driverSecondYearActualThread.Join();

            driverThirdYearForecastThread.Start();
            driverThirdYearForecastThread.Join();


            driverFourthYearForecastThread.Start();
            driverFourthYearForecastThread.Join();

            driverFifthYearForecastThread.Start();
            driverFifthYearForecastThread.Join();



            List<TearSheetOutput> tearsheetMappings_T_1 = allTearsheetOutputs.Where(x => x.FinancialYearId == 1).ToList();
            List<TearSheetOutput> tearsheetMappings_T_2 = allTearsheetOutputs.Where(x => x.FinancialYearId == 2).ToList();
            List<TearSheetOutput> tearsheetMappings_T_3 = allTearsheetOutputs.Where(x => x.FinancialYearId == 3).ToList();
            List<TearSheetOutput> tearsheetMappings_T_4 = allTearsheetOutputs.Where(x => x.FinancialYearId == 4).ToList();
            List<TearSheetOutput> tearsheetMappings_T_5 = allTearsheetOutputs.Where(x => x.FinancialYearId == 5).ToList();

            List<KeyValuePair<string, object>> results_T_1 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_2 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_3 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_4 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_5 = new List<KeyValuePair<string, object>>();

            Thread firstYearActualThread = new Thread(() =>
            {
                results_T_1 = CalculateDynamicService.GetOriginalTearSheetData(tearsheetMappings_T_1, workbook);


            });

            Thread secondYearActualThread = new Thread(() =>
                           {
                               results_T_2 = CalculateDynamicService.GetOriginalTearSheetData(tearsheetMappings_T_2, workbook);


                           });

            Thread thirdYearForecastThread = new Thread(() =>
            {


                results_T_3 = CalculateDynamicService.GetOriginalTearSheetData(tearsheetMappings_T_3, workbook);


            });

            Thread fourthYearForecastThread = new Thread(() =>
            {


                results_T_4 = CalculateDynamicService.GetOriginalTearSheetData(tearsheetMappings_T_4, workbook);


            });

            Thread fifthYearForecastThread = new Thread(() =>
                           {


                               results_T_5 = CalculateDynamicService.GetOriginalTearSheetData(tearsheetMappings_T_5, workbook);


                           });

            firstYearActualThread.Start();
            firstYearActualThread.Join();
            secondYearActualThread.Start();
            secondYearActualThread.Join();
            thirdYearForecastThread.Start();
            thirdYearForecastThread.Join();
            fourthYearForecastThread.Start();
            fourthYearForecastThread.Join();
            fifthYearForecastThread.Start();
            fifthYearForecastThread.Join();

            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Summary", resultsSummaryTearSheetValue);
            result.Add("T-1", results_T_1.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T", results_T_2.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+1", results_T_3.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+2", results_T_4.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+3", results_T_5.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("firstYearActualDriver", driverResultsFirstYearActual.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("secondYearActualDriver", driverResultsSecondYearActual.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("thirdYearForecastDriver", driverResultsThirdYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("fourthYearForecastDriver", driverResultsFourthYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("fifthYearForecastDriver", driverResultsFifthYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));



            return result;


        }
        public static async Task<object> CalculateDynamicInputValues(ExcelFile workbook, DynamicInputsBody request, StockUpload stockUpload, IEnumerable<DriverTearSheetOutput> allDriverTearSheets, List<SummaryTearSheetOutput> summaryTearSheetOutputs, List<TearSheetOutput> allTearsheetOutputs)
        {

            SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");



            List<KeyValuePair<string, object>> resultsSummaryTearSheetValue = new List<KeyValuePair<string, object>>();





            workbook = CalculateDynamicService.UpdateDriverInputs(request.driverInputs, workbook, allDriverTearSheets);


            List<DriverTearSheetOutput> driverMappingFirstYearActual = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 1).ToList();
            List<DriverTearSheetOutput> driverMappingSecondYearActual = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 2).ToList();
            List<DriverTearSheetOutput> driverMappingThirdYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 3).ToList();
            List<DriverTearSheetOutput> driverMappingFourthYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 4).ToList();
            List<DriverTearSheetOutput> driverMappingFifthYearForecast = stockUpload.DriverTearSheetOutputs.Where(x => x.FinancialYearId == 5).ToList();

            List<KeyValuePair<string, object>> driverResultsFirstYearActual = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsSecondYearActual = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsThirdYearForecast = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsFourthYearForecast = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> driverResultsFifthYearForecast = new List<KeyValuePair<string, object>>();

            Thread firstYearActualThreadDriver = new Thread(() =>
            {
                driverResultsFirstYearActual = CalculateDynamicService.CalculateDynamicDriverTearSheet(driverMappingFirstYearActual, workbook);


            });

            Thread secondYearActualThreadDriver = new Thread(() =>

            {
                driverResultsSecondYearActual = CalculateDynamicService.CalculateDynamicDriverTearSheet(driverMappingSecondYearActual, workbook);

            });


            Thread thirdYearForecastThreadDriver = new Thread(() =>

            {
                driverResultsThirdYearForecast = CalculateDynamicService.CalculateDynamicDriverTearSheet(driverMappingThirdYearForecast, workbook);

            });


            Thread fourthYearForecastThreadDriver = new Thread(() =>

                       {
                           driverResultsFourthYearForecast = CalculateDynamicService.CalculateDynamicDriverTearSheet(driverMappingFourthYearForecast, workbook);

                       });

            Thread fifthYearForecastThreadDriver = new Thread(() =>

                                  {
                                      driverResultsFifthYearForecast = CalculateDynamicService.CalculateDynamicDriverTearSheet(driverMappingFifthYearForecast, workbook);

                                  });

            firstYearActualThreadDriver.Start();
            firstYearActualThreadDriver.Join();
            secondYearActualThreadDriver.Start();
            thirdYearForecastThreadDriver.Start();
            secondYearActualThreadDriver.Join();
            thirdYearForecastThreadDriver.Join();
            fourthYearForecastThreadDriver.Start();
            fourthYearForecastThreadDriver.Join();
            fifthYearForecastThreadDriver.Start();
            fifthYearForecastThreadDriver.Join();





            List<TearSheetOutput> tearsheetMappings_T_1 = allTearsheetOutputs.Where(x => x.FinancialYearId == 1).ToList();
            List<TearSheetOutput> tearsheetMappings_T_2 = allTearsheetOutputs.Where(x => x.FinancialYearId == 2).ToList();
            List<TearSheetOutput> tearsheetMappings_T_3 = allTearsheetOutputs.Where(x => x.FinancialYearId == 3).ToList();
            List<TearSheetOutput> tearsheetMappings_T_4 = allTearsheetOutputs.Where(x => x.FinancialYearId == 4).ToList();
            List<TearSheetOutput> tearsheetMappings_T_5 = allTearsheetOutputs.Where(x => x.FinancialYearId == 5).ToList();


            // workbook = CalculateDynamicService.UpdateTearSheetInputs(request.cellInputs, workbook, tearsheetMappings_T_1);
            // workbook = CalculateDynamicService.UpdateTearSheetInputs(request.cellInputs, workbook, tearsheetMappings_T_2);
            // workbook = CalculateDynamicService.UpdateTearSheetInputs(request.cellInputs, workbook, tearsheetMappings_T_3);
            // workbook = CalculateDynamicService.UpdateTearSheetInputs(request.cellInputs, workbook, tearsheetMappings_T_4);
            // workbook = CalculateDynamicService.UpdateTearSheetInputs(request.cellInputs, workbook, tearsheetMappings_T_5);

            List<TearSheetReference> GrossMarginEntries = request.cellInputs.Where(x => x.name == "Gross Margin").ToList();
            bool isGrossMarginAnInput = true;

            if (GrossMarginEntries.Count == 0)
            {
                isGrossMarginAnInput = false;

            }



            foreach (TearSheetReference cellInput in request.cellInputs)
            {

                if (cellInput.financialYearId == 1)
                {


                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_1.Where(x => x.Name == cellInput.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = cellInput.value.ToString();


                }

                if (cellInput.financialYearId == 2)
                {


                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_2.Where(x => x.Name == cellInput.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = cellInput.value.ToString();


                }

                if (cellInput.financialYearId == 3)
                {


                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_3.Where(x => x.Name == cellInput.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = cellInput.value.ToString();


                }



                if (cellInput.financialYearId == 4)
                {


                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_4.Where(x => x.Name == cellInput.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = cellInput.value.ToString();


                }

                if (cellInput.financialYearId == 5)

                {

                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_5.Where(x => x.Name == cellInput.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = cellInput.value.ToString();

                }


            }



            List<KeyValuePair<string, object>> results_T_1 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_2 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_3 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_4 = new List<KeyValuePair<string, object>>();
            List<KeyValuePair<string, object>> results_T_5 = new List<KeyValuePair<string, object>>();

            Thread firstYearActualThread = new Thread(() => { results_T_1 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_1, workbook, 1, isGrossMarginAnInput); });
            Thread secondYearActualThread = new Thread(() => { results_T_2 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_2, workbook, 2, isGrossMarginAnInput); });
            Thread thirdYearForecastThread = new Thread(() => { results_T_3 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_3, workbook, 3, isGrossMarginAnInput); });
            Thread fourthYearForecastThread = new Thread(() => { results_T_4 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_4, workbook, 4, isGrossMarginAnInput); });
            Thread fifthYearForecastThread = new Thread(() => { results_T_5 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_5, workbook, 5, isGrossMarginAnInput); });

            firstYearActualThread.Start();
            firstYearActualThread.Join();
            secondYearActualThread.Start();
            secondYearActualThread.Join();
            thirdYearForecastThread.Start();
            thirdYearForecastThread.Join();
            fourthYearForecastThread.Start();
            fourthYearForecastThread.Join();
            fifthYearForecastThread.Start();
            fifthYearForecastThread.Join();

            Dictionary<string, object> result = new Dictionary<string, object>();

            result.Add("Summary", resultsSummaryTearSheetValue);
            result.Add("T-1", results_T_1.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T", results_T_2.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+1", results_T_3.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+2", results_T_4.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("T+3", results_T_5.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("firstYearActualDriver", driverResultsFirstYearActual.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("secondYearActualDriver", driverResultsSecondYearActual.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("thirdYearForecastDriver", driverResultsThirdYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("fourthYearForecastDriver", driverResultsFourthYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));
            result.Add("fifthYearForecastDriver", driverResultsFifthYearForecast.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value));


            return result;


        }
    }


}

