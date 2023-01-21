using System;
using backend_engine.Models;
using Microsoft.EntityFrameworkCore;

namespace backend_engine.Repositories

{
    public class StockRepository : Repository<Stock>, IStockRepository
    {

        public StockRepository(BreezeDataContext context) : base(context)
        {


        }

        public async Task<object> GetStockWithStockUploads(int Id)
        {

            object stock = await _context.Stocks.Where(x => x.Id == Id).Include(x => x.StockUploads.OrderByDescending(t => t.UploadedAt)).ThenInclude(c => c.StockTearSheetOutputs).ToListAsync();
            return stock;


        }



    }







}