
using LightSaver.Infrastructure.Helpers;
using LightSaver.Models;
using Newtonsoft.Json;

namespace LightSaver.Services
{
    //Service that is going to provide us the way of making calls to the external REE API
    public class REEService
    {

        private HttpClient _httpClient;

        public REEService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task<List<Price>> GetREEData(DateTime date)
        {

            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            Console.WriteLine(day+"/"+month+"/"+year);
            Console.WriteLine(date.ToString());

            

            string URL = "https://api.esios.ree.es/archives/70/download_json?locale=es&date="+year+"-"+month+"-"+day;

            var request = new HttpRequestMessage(HttpMethod.Get, URL); //creo la request con el método que usaré que es GET y la URL
            
            //request.Headers.Add("Accept", "application/json;application/vnd.esios-api-v1+json");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Host", "api.esios.ree.es");
            request.Headers.Add("Authorization", "Token token=16388be3b19d1f03112ab963f5b0660f07abfe1d616859d5e8f032074b25093d");
            request.Headers.Add("Cookie", "");


            var response = await _httpClient.SendAsync(request);
            //var response = await _httpClient.GetAsync(URL); otra opción

            string responseJSON = await response.Content.ReadAsStringAsync(); //Saves the content as a string JSON


            List<Price> dailyPrices = parseJSON(responseJSON, date); //Parses the JSON into a List of Objects of the type Price

          

            return dailyPrices;
        }



        /**
         * The method parses the JSON response into ResponseOBJ object.
         * Then the interesting values are taken into a Price Object´s List.
         */

        private List<Price> parseJSON(string responseJSON, DateTime pricesDate)
        {
            long AutoIncrementalNo = 1;
            ResponseREE deserialized = JsonConvert.DeserializeObject<ResponseREE>(responseJSON);
            //Console.WriteLine(deserialized.PVPC[0].Dia);

            List<Price> dailyPrices = new List<Price>();

            for (int i = 0; i < deserialized.PVPC.Count; i++)
            {
                Price dailyPrice = new Price();

                dailyPrice.Id = $"{DateTimeHelper.ToPriceDate(pricesDate)}-{AutoIncrementalNo}";
                // dailyPrice.Id = DateTimeHelper.ToPriceDate(pricesDate) +"-"+AutoIncrementalNo; //The id of each price is composed by its date concatenadted with - and an autoIncremental No
                string[] date = deserialized.PVPC[i].Dia.Split('/'); 
                DateTime formattedDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]));

                string[] interval = deserialized.PVPC[i].Hora.Split('-');



                dailyPrice.PriceDate = formattedDate;
                dailyPrice.IntervalStart = int.Parse(interval[0]);
                dailyPrice.IntervalEnd = int.Parse(interval[1]);
                dailyPrice.MWHPricePCB = deserialized.PVPC[i].PCB;
                dailyPrice.MWHPriceCYM = deserialized.PVPC[i].CYM;


                dailyPrices.Add(dailyPrice);
                AutoIncrementalNo++;
            }


            return dailyPrices;

        }
    }
    }
