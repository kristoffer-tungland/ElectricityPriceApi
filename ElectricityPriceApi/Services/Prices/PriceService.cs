using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;

namespace ElectricityPriceApi.Services.Prices
{
    internal class PriceService : IPriceService
    {
        private readonly EntsoeHttpClient _entsoeHttpClient;

        public PriceService(EntsoeHttpClient entsoeHttpClient)
        {
            _entsoeHttpClient = entsoeHttpClient;
        }

        public Task<GetHourPricesResult> GetHourPrices(GetHourPricesArgs args)
        {
            return _entsoeHttpClient.GetHourPrices(args);
        }
    }
}