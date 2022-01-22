using LightSaver.Models;
using LightSaver.Infrastructure.DAO;

namespace LightSaver.Services
{
    public class RefreshPricesService : IHostedService
    {


        private readonly ILogger<RefreshPricesService> _log;
        private readonly REEService _reeService;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private volatile bool isRefreshing = false;
        private const int TimerInterval = 3;


        public RefreshPricesService(ILogger<RefreshPricesService> logger, REEService myservice, IServiceProvider serviceProvider)
        {
            _log = logger;
            _reeService = myservice;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(RefreshPricesAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(TimerInterval));

            _log.LogInformation("LOG: Refresh Prices Service timer interval set up to {timerVal} minutes", TimerInterval);

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            _timer.Change(Timeout.Infinite, 0);
            while (isRefreshing)
            {
                Thread.Sleep(2000); //sleeps for two seconds so that the current task (if exists) can finish without problems
            }

            return Task.CompletedTask;

        }


        private async void RefreshPricesAsync(object? state)
        {
            _log.LogInformation("LOG: Refresh Prices Service started {date}", DateTime.Now);

            isRefreshing = true;

            //First of all, we´ll obtain the new prices for today

            DateTime today = DateTime.Now;

            //Constructor is overloaded; -> public DateTime(int year, int month, int day)
            //DateTime customDate = new DateTime(2021, 11, 26);


            List<Price> updatedPrices = await _reeService.GetREEData(today);
            _log.LogInformation("LOG: Updated prices correctly retrieved from REE API");


            List<Price> storedPrices = null;
            PricesDAO dbAccess = null;


            //Since the Background service is a Singleton and the injected dependency PricesDAO is Transient along with the db context that it uses eventually, this contitutes the parent service of both of them
            //That said, the childs cannot be transient or scoped with a singleton parent so we must fake an scope so that it can work without exceptions

            using (var scope = _serviceProvider.CreateScope())
            {
                dbAccess = scope.ServiceProvider.GetRequiredService<PricesDAO>();

                //Then, we´ll check if they have changed or not in comparison to those saved already, so we need to retrieve them.

                storedPrices = await dbAccess.GetPrices(today); //probar que pasa si no existe entrada para el dia de hoy (caso primera vez)
                _log.LogInformation("LOG: Saved prices correctly queried from DataBase");




                //If it is the case in which no prices are saved in the DB (because its the first execution or a new day) they must be saved straightforward

                if (storedPrices == null)
                {
                    await dbAccess.SavePrices(updatedPrices);
                    _log.LogInformation("LOG: New Prices succesfully saved for the fist time on date {date}", DateTime.Now);

                }
                else if (updatedPrices.SequenceEqual(storedPrices) == false)
                {
                    //Now we compare the lists of prices inside both of them
                    //if they are not equal, we update the old ones with the new ones

                    for (int i = 0; i < updatedPrices.Count; i++)
                    {
                        dbAccess.UpdatePrices(updatedPrices[i]);

                    }
                    _log.LogInformation("LOG: Prices in DataBase succesfully updated with the new ones retrieved");





                }

                _log.LogInformation("LOG: Service Completed");
                isRefreshing = false;

            }

        }


    }
}
