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

            List<TearSheetOutput> allTearsheetOutputs = _memoryCache.Get<List<TearSheetOutput>>("TearSheetMappings");

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

                List<SummaryTearSheetOutput> summaryTearSheetOutputs = _memoryCache.Get<List<SummaryTearSheetOutput>>("SummarySheetMappings");
                //List<SummaryTearSheetOutput> summaryTearSheetOutputs = await _context.SummaryTearSheetOutputs.ToListAsync();
                if (summaryTearSheetOutputs.Count == 0)
                {
                    return NotFound("Stock upload could not be located");
                }

                object result = CalculateDynamicService.CalculateTearSheet(workbook, summaryTearSheetOutputs, stockUpload, allTearsheetOutputs);



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

            List<SummaryTearSheetOutput> summaryTearSheetOutputs = _memoryCache.Get<List<SummaryTearSheetOutput>>("SummarySheetMappings");

            List<KeyValuePair<string, object>> resultsSummaryTearSheetValue = new List<KeyValuePair<string, object>>();

            List<TearSheetOutput> allTearsheetOutputs = _memoryCache.Get<List<TearSheetOutput>>("TearSheetMappings");





            object result = CalculateDynamicService.CalculateDynamicInputValues(workbook, request, stockUpload, allDriverTearSheets, summaryTearSheetOutputs, allTearsheetOutputs);

            return result;





        }
    }
}




