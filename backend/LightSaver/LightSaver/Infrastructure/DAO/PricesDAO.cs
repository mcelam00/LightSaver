using LightSaver.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LightSaver.Infrastructure.DAO
{

    //Data Access Object that communicates with the DB via the context (low coupling high cohesion)
    public class PricesDAO
    {

        private readonly MyDbContext _context;

        public PricesDAO(MyDbContext context)
        {
            _context = context;
        }

        // Returns all the Prices objects in the DB
        public async Task<IEnumerable<Price>> GetAllPrices()
        {
            return await _context.Prices.ToListAsync();
        }


        // Returns specific Prices for the requested date/ null of no entries for that date
        public async Task<List<Price>> GetPrices(DateTime date)
        {
            List<Price> prices = new List<Price>();

            string formattedDateAndHour = DateTimeHelper.ToPriceDate(date);
            string [] formattedDate = formattedDateAndHour.Split(' ');

            string formatted = "";
            Price price = null;

            for (int i = 1; i<=24; i++)
            {

                formatted = formattedDate[0] + "-" + i;
                price = _context.Prices.Find(formatted);
                if (price == null)
                {
                    prices = null;
                    break;
                }

                prices.Add(price);
            }

            return prices;

        }


        // Saves the Prices object to the DB
        public async Task<bool> SavePrices(List<Price> dailyPricesOBJ)
        {
            for (int i = 0; i < dailyPricesOBJ.Count; i++)
            {
                //with the context, I call to the DbSet that I have (obj that represents the hole DB), whose name is Prices of type DbSet<Price> and I add the first List of Price objects for the specific date QueryDate
                _context.Prices.Add(dailyPricesOBJ[i]);
                await _context.SaveChangesAsync();

            }



            return true; //Temporary
        }



        // Updates the Prices object in the DB
        public async Task<bool> UpdatePrices(Price dailyPrice)
        {
            bool ok = true;

            if (_context.Prices.Find(dailyPrice.Id) == null)
            {//if the entity is not saved already we save it directly

                _context.Prices.Add(dailyPrice);
                await _context.SaveChangesAsync();


            }
            else
            {//we update it with the new value
                _context.Entry(dailyPrice).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    //todo


                }
            }



            return ok;
        }




    }
}
