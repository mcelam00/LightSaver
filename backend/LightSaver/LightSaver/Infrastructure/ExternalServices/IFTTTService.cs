namespace LightSaver.Services
{
    //Service that is going to allow us to interact remotely with the smart plug (On/Off)
    public class IFTTTService
    {

        private HttpClient _httpClient;

        public IFTTTService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task<Boolean> TurnPlugOn()
        {
            Boolean success = true; //TODO

            
            string URL = "https://maker.ifttt.com/trigger/Enchufar/with/key/dje2e2aNtKamVk_VWp90t9";

            await _httpClient.GetAsync(URL);

            return success;

        }

        public async Task<Boolean> TurnPlugOff()
        {
            Boolean success = true;//TODO

            string URL = "https://maker.ifttt.com/trigger/Apagar/with/key/dje2e2aNtKamVk_VWp90t9";

            await _httpClient.GetAsync(URL);

            return success;

        }

    }
}
