using LightSaver.Infrastructure.DAO;

namespace LightSaver.Domain.Services
{

    /*Service responsible for retrieving the prices from the Database*/

    public class PricesService
    {
        private readonly PricesDAO _dbAcess;

        public PricesService(PricesDAO dbAccess)
        {
            _dbAcess = dbAccess;

        }


        public async Task<IEnumerable<Price>> GetPrices()
        {
            return await _dbAcess.GetAllPrices();

        }


        public async Task<IEnumerable<Price>> GetDatePrices(DateTime date)
        {
            return await _dbAcess.GetPrices(date);
        }




    }
}
