using backend_engine.Models;

namespace backend_engine.Repositories
{

    public interface IStockRepository : IRepository<Stock>
    {

        public Task<object> GetStockWithStockUploads(int Id);
        public Task<object> GetAllStocksWithStockUploads();


    }


}