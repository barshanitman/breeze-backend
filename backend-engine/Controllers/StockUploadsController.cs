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
using MongoDB.Driver.Core.Configuration;
using backend_engine.RequestBodies;
using backend_engine.ResponseBodies;
using backend_engine.Repositories;

namespace backend_engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockUploadsController : ControllerBase
    {
        private readonly BreezeDataContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=hermesworkbooks;AccountKey=EaVNOEBoTyhe+S330Ec670TN0WlqHPdBNJ+Yp6eUB0q1XWGrpl/xE5GH0aVciVey42PRn+ZZSP0l+AStM9JbHg==;EndpointSuffix=core.windows.net";
        private const string containerName = "hermes-workbooks";
        private readonly IRepository<TearSheetOutput> _tearSheetOutputRepo;
        private readonly IRepository<StockUploadTearsheetValue> _stockUploadTearSheetValueRepo;

        public StockUploadsController(BreezeDataContext context, IMemoryCache memorycache, IRepository<TearSheetOutput> tearSheetOutputRepo,
        IRepository<StockUploadTearsheetValue> stockUploadTearSheetValueRepo

        )
        {
            _context = context;
            _memoryCache = memorycache;
            _tearSheetOutputRepo = tearSheetOutputRepo;
            _stockUploadTearSheetValueRepo = stockUploadTearSheetValueRepo;

        }

        // GET: api/StockUploads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockUpload>>> GetStockUploads()
        {
            if (_context.StockUploads == null)
            {
                return NotFound();
            }
            return await _context.StockUploads.ToListAsync();
        }

        // GET: api/StockUploads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockUpload>> GetStockUpload(int id)
        {
            if (_context.StockUploads == null)
            {
                return NotFound();
            }
            var stockUpload = await _context.StockUploads.FindAsync(id);

            if (stockUpload == null)
            {
                return NotFound();
            }

            return stockUpload;
        }

        [HttpGet("/api/StockUploads/Stock/{id}")]
        public async Task<ActionResult<object>> GetStockUploadByStockId(int id)
        {

            object stockUpload = _context.StockUploads.Where(x => x.StockId == id).OrderByDescending(t => t.UploadedAt);

            if (stockUpload == null)
            {
                return NotFound();
            }

            return stockUpload;
        }

        [HttpGet("/api/StockUploads/Active/Stock/{id}")]
        public async Task<ActionResult<object>> GetStockUploadByActiveStockId(int id)
        {

            object stockUpload = _context.StockUploads.Where(x => x.StockId == id).OrderByDescending(t => t.UploadedAt).FirstOrDefault();

            if (stockUpload == null)

            {
                return NotFound();
            }

            return stockUpload;
        }

        // PUT: api/StockUploads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockUpload(int id, StockUpload stockUpload)
        {
            if (id != stockUpload.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockUpload).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockUploadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StockUploads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StockUpload>> PostStockUpload(StockUpload stockUpload)
        {
            if (_context.StockUploads == null)
            {
                return Problem("Entity set 'BreezeDataContext.StockUploads'  is null.");
            }
            _context.StockUploads.Add(stockUpload);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetStockUpload", new { id = stockUpload.Id }, stockUpload);
        }

        // DELETE: api/StockUploads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockUpload(int id)
        {
            if (_context.StockUploads == null)
            {
                return NotFound();
            }
            var stockUpload = await _context.StockUploads.FindAsync(id);

            if (stockUpload == null)
            {
                return NotFound();
            }

            _context.StockUploads.Remove(stockUpload);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockUploadExists(int id)
        {
            return (_context.StockUploads?.Any(e => e.Id == id)).GetValueOrDefault();
        }









        [HttpPost("/StockUpload/Calculate/{id}")]
        public async Task<object> CalculateTearSheet(int id)
        {


            SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");
            Dictionary<string, object> calcResult = _memoryCache.Get<Dictionary<string, object>>("calculation" + id.ToString());

            if (calcResult == null)
            {


                StockUpload stockUpload = await _context.StockUploads.Where(x => x.Id == id).Include(x => x.DriverTearSheetOutputs).FirstAsync();


                if (stockUpload == null)
                {
                    return NotFound();
                }
                ExcelFile workbook = _memoryCache.Get<ExcelFile>(stockUpload.FileName);

                if (workbook is null)
                {
                    BlobClient blobClient = new BlobClient(connectionString, containerName, stockUpload.FileName);
                    var stream = new MemoryStream();
                    blobClient.DownloadTo(stream);
                    workbook = ExcelFile.Load(stream);
                    workbook = PreprocessWorkbook.PreProcessFile(workbook);
                    _memoryCache.Set(stockUpload.FileName, workbook, TimeSpan.FromHours(24));
                    stream.Close();

                }

                List<KeyValuePair<string, object>> resultsSummaryTearSheetValue = new List<KeyValuePair<string, object>>();
                List<SummaryTearSheetOutput> summaryTearSheetOutputs = _memoryCache.Get<List<SummaryTearSheetOutput>>("SummarySheetMappings");
                //List<SummaryTearSheetOutput> summaryTearSheetOutputs = await _context.SummaryTearSheetOutputs.ToListAsync();
                if (summaryTearSheetOutputs.Count == 0)
                {
                    return NotFound();
                }


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


                List<TearSheetOutput> allTearsheetOutputs = _memoryCache.Get<List<TearSheetOutput>>("TearSheetMappings");

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


                // foreach (TearSheetOutput map in tearsheetMappings_T_1)

                // {

                //     results_T_1.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                // }


                // foreach (TearSheetOutput map in tearsheetMappings_T_2)

                // {

                //     results_T_2.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                // }

                // foreach (TearSheetOutput map in tearsheetMappings_T_3)

                // {

                //     results_T_3.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                // }

                // foreach (TearSheetOutput map in tearsheetMappings_T_4)

                // {

                //     results_T_4.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                // }


                // foreach (TearSheetOutput map in tearsheetMappings_T_5)

                // {

                //     results_T_5.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                // }



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


                _memoryCache.Set("calculation" + id.ToString(), result, TimeSpan.FromHours(48));

                return result;

            }

            else

            {
                return calcResult;
            }

        }


        [HttpPost("/StockUpload/CalculateDynamicInputs")]
        public async Task<object> CalculateTearSheetDynamicInputs([FromBody] DynamicInputsBody request)
        {


            SpreadsheetInfo.SetLicense("SN-2022Dec14-8dsaQkUuOJsK9mB7z329lSTP9Re69lgZv8e3hfz7b8MeGQ89HmAgjhHwkBr7fW0CagUGOTdUhyb5AAd/RQTCPShAtug==A");
            Dictionary<string, object> calcResult = _memoryCache.Get<Dictionary<string, object>>("calculation" + request.workbookId.ToString());


            StockUpload stockUpload = await _context.StockUploads.Where(x => x.Id == request.workbookId).FirstOrDefaultAsync();
            if (stockUpload == null)
            {
                return NotFound();

            }

            IEnumerable<DriverTearSheetOutput> allDriverTearSheets = await _context.DriverTearSheetOutputs.Where(x => x.StockUploadId == stockUpload.Id).ToListAsync();


            ExcelFile originalworkbook = _memoryCache.Get<ExcelFile>(stockUpload.FileName);


            if (originalworkbook is null)
            {
                BlobClient blobClient = new BlobClient(connectionString, containerName, stockUpload.FileName);
                var stream = new MemoryStream();
                blobClient.DownloadTo(stream);
                originalworkbook = ExcelFile.Load(stream);
                originalworkbook = PreprocessWorkbook.PreProcessFile(originalworkbook);
                _memoryCache.Set(stockUpload.FileName, originalworkbook, TimeSpan.FromHours(24));
                stream.Close();
                Console.WriteLine("Downloaded file from blob");


            }

            ExcelFile workbook = originalworkbook.Clone();

            List<KeyValuePair<string, object>> resultsSummaryTearSheetValue = new List<KeyValuePair<string, object>>();

            List<SummaryTearSheetOutput> summaryTearSheetOutputs = _memoryCache.Get<List<SummaryTearSheetOutput>>("SummarySheetMappings");
            //List<SummaryTearSheetOutput> summaryTearSheetOutputs = await _context.SummaryTearSheetOutputs.ToListAsync();

            //Drivers stuff 


            IEnumerable<DriverTearSheetOutput> firstYearActualLookup = allDriverTearSheets.Where(x => x.FinancialYearId == 1);
            IEnumerable<DriverTearSheetOutput> secondYearActualLookup = allDriverTearSheets.Where(x => x.FinancialYearId == 2);
            IEnumerable<DriverTearSheetOutput> thirdYearForecastLookup = allDriverTearSheets.Where(x => x.FinancialYearId == 3);
            IEnumerable<DriverTearSheetOutput> fourthYearForecastLookup = allDriverTearSheets.Where(x => x.FinancialYearId == 4);
            IEnumerable<DriverTearSheetOutput> fifthYearForecastLookup = allDriverTearSheets.Where(x => x.FinancialYearId == 5);

            foreach (DriverReference driverInput in request.driverInputs)
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



            foreach (DriverTearSheetOutput map in driverMappingFirstYearActual)

            {
                if (map.IsFormula)
                {

                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    driverResultsFirstYearActual.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                }
                else
                {

                    driverResultsFirstYearActual.Add(new KeyValuePair<string, object>(map.Name, 0));

                }

            }


            foreach (DriverTearSheetOutput map in driverMappingSecondYearActual)

            {
                if (map.IsFormula)
                {

                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    driverResultsSecondYearActual.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                }

                else
                {

                    driverResultsSecondYearActual.Add(new KeyValuePair<string, object>(map.Name, 0));

                }


            }

            foreach (DriverTearSheetOutput map in driverMappingThirdYearForecast)

            {
                if (map.IsFormula)
                {

                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    driverResultsThirdYearForecast.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                }

                else
                {


                    driverResultsThirdYearForecast.Add(new KeyValuePair<string, object>(map.Name, 0));



                }


            }


            foreach (DriverTearSheetOutput map in driverMappingFourthYearForecast)

            {
                if (map.IsFormula)
                {


                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    driverResultsFourthYearForecast.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));

                }
                else
                {


                    driverResultsFourthYearForecast.Add(new KeyValuePair<string, object>(map.Name, 0));

                }

            }


            foreach (DriverTearSheetOutput map in driverMappingFifthYearForecast)

            {
                if (map.IsFormula)
                {

                    workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Calculate();
                    driverResultsFifthYearForecast.Add(new KeyValuePair<string, object>(map.Name, workbook.Worksheets[map.SheetReference.ToString()].Cells[map.CellReference.ToString()].Value));


                }
                else
                {

                    driverResultsFifthYearForecast.Add(new KeyValuePair<string, object>(map.Name, 0));


                }


            }

            List<TearSheetOutput> allTearsheetOutputs = _memoryCache.Get<List<TearSheetOutput>>("TearSheetMappings");




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

            foreach (TearSheetReference inputCell in request.cellInputs)

            {
                if (inputCell.financialYearId == 1)
                {
                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_1.Where(x => x.Name == inputCell.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();



                }
                else if (inputCell.financialYearId == 2)
                {

                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_2.Where(x => x.Name == inputCell.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();



                }

                else if (inputCell.financialYearId == 3)
                {

                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_3.Where(x => x.Name == inputCell.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();



                }

                else if (inputCell.financialYearId == 4)
                {

                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_4.Where(x => x.Name == inputCell.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();



                }

                else if (inputCell.financialYearId == 5)
                {

                    TearSheetOutput CellLocationOfItem = tearsheetMappings_T_5.Where(x => x.Name == inputCell.name).First();
                    workbook.Worksheets[CellLocationOfItem.SheetReference].Cells[CellLocationOfItem.CellReference].Formula = inputCell.value.ToString();

                }



            }

            Thread firstYearActualThread = new Thread(() => { results_T_1 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_1, workbook); });
            Thread secondYearActualThread = new Thread(() => { results_T_2 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_2, workbook); });
            Thread thirdYearForecastThread = new Thread(() => { results_T_3 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_3, workbook); });
            Thread fourthYearForecastThread = new Thread(() => { results_T_4 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_4, workbook); });
            Thread fifthYearForecastThread = new Thread(() => { results_T_5 = CalculateDynamicService.CalculateFinancialYearTearSheet(tearsheetMappings_T_5, workbook); });

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




