using System;
using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;

namespace ElectricityPriceApi.Services.Prices
{
    internal class PriceService : IPriceService
    {
        private readonly EntsoeHttpClient _entsoeHttpClient;
        private readonly NorskeBankHttpClient _norskeBankHttpClient;

        public PriceService(EntsoeHttpClient entsoeHttpClient, NorskeBankHttpClient norskeBankHttpClient)
        {
            _entsoeHttpClient = entsoeHttpClient;
            _norskeBankHttpClient = norskeBankHttpClient;
        }

        public async Task<GetHourPricesResult> GetHourPrices(GetHourPricesArgs args)
        {
            var result = await _entsoeHttpClient.GetHourPrices(args);

            var toCurrency = args.Currency;
            var fromCurrency = result.CurrencyUnitName;

            if (fromCurrency == null)
                throw new NullReferenceException("From currency was not set");

            if (toCurrency == fromCurrency)
                return result;

            var nokCurrency = "NOK";

            var exchangeRateArgs = new ExchangeRateArgs(args.PeriodEnd.AddDays(-7), args.PeriodEnd, args.Area, fromCurrency, nokCurrency);

            //Todo If currency is not NOK, do another call to get

            var exchangeRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeRateArgs);

            var exchangeRate = exchangeRateResult.Observation;

            if (toCurrency != nokCurrency)
            {
                var exchangeToNotNokRateArgs = new ExchangeRateArgs(args.PeriodEnd.AddDays(-7), args.PeriodEnd, args.Area, toCurrency, nokCurrency);
                var exchangeToNotNokRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeToNotNokRateArgs);

                // TODO DKK to NOK gets strange results
                var exchange = 1 / exchangeToNotNokRateResult.Observation;

                exchangeRate *= exchange;
            }

            result.CurrencyUnitName = toCurrency;
            result.Prices?.ForEach(x => x.Price *= exchangeRate);

            return result;
        }
    }
}