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

            const string nokCurrency = "NOK";

            var exchangeToNokRateArgs = new ExchangeRateArgs(args.PeriodEnd.AddDays(-7), args.PeriodEnd, args.Area, fromCurrency, nokCurrency);
            var exchangeToNokRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeToNokRateArgs);

            var exchangeRate = exchangeToNokRateResult.ExchangeRate;

            if (toCurrency != nokCurrency)
            {
                var exchangeFromNokRateArgs = new ExchangeRateArgs(args.PeriodEnd.AddDays(-7), args.PeriodEnd, args.Area, toCurrency, nokCurrency);
                var exchangeFromNokRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeFromNokRateArgs);

                var exchangeFromNok = 1 / exchangeFromNokRateResult.ExchangeRate;

                exchangeRate *= exchangeFromNok;
            }

            result.CurrencyUnitName = toCurrency;
            result.Prices?.ForEach(x => x.Price *= exchangeRate);

            return result;
        }
    }
}