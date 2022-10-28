using System;
using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;

namespace ElectricityPriceApi.Services.Prices;

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
        var getHourPricesResult = await _entsoeHttpClient.GetHourPrices(args);

        var toCurrency = args.Currency;
        var fromCurrency = getHourPricesResult.CurrencyUnitName;

        if (fromCurrency == null)
            throw new NullReferenceException("From currency was not set");

        if (toCurrency == fromCurrency)
            return getHourPricesResult;

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

        getHourPricesResult.CurrencyUnitName = toCurrency;
        getHourPricesResult.Prices?.ForEach(x => x.Price *= exchangeRate);

        return getHourPricesResult;
    }
}