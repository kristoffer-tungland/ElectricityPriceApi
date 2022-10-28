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

        await CalculateExchangeRates(args.Currency, args.PeriodEnd, args.Area, getHourPricesResult);

        return getHourPricesResult;
    }

    private async Task CalculateExchangeRates(string currency, DateTime date, Area area, GetHourPricesResult getHourPricesResult)
    {
        //Todo Improve exchange rates to follow the dates for prices, don't use current always!
        var fromCurrency = getHourPricesResult.CurrencyUnitName;

        if (fromCurrency == null)
            throw new NullReferenceException("From currency was not set");

        if (currency == fromCurrency)
            return;

        const string nokCurrency = "NOK";

        var exchangeToNokRateArgs = new ExchangeRateArgs(date.AddDays(-7), date, area, fromCurrency, nokCurrency);
        var exchangeToNokRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeToNokRateArgs);

        var exchangeRate = exchangeToNokRateResult.ExchangeRate;

        if (currency != nokCurrency)
        {
            var exchangeFromNokRateArgs = new ExchangeRateArgs(date.AddDays(-7), date, area, currency, nokCurrency);
            var exchangeFromNokRateResult = await _norskeBankHttpClient.GetExchangeRate(exchangeFromNokRateArgs);

            var exchangeFromNok = 1 / exchangeFromNokRateResult.ExchangeRate;

            exchangeRate *= exchangeFromNok;
        }

        getHourPricesResult.CurrencyUnitName = currency;
        getHourPricesResult.Prices?.ForEach(x => x.Price *= exchangeRate);
    }

    public async Task<GetAveragePricesResult?> GetAveragePrices(GetAveragePricesArgs args)
    {
        var localTime = args.Area.LocalTimeNow().AddDays(1);
        var end = new DateTime(localTime.Year, localTime.Month, localTime.Day);
        var start = end.AddDays(-31);
        var getHourPricesArgs = new GetHourPricesArgs(args.Area, start, end, args.Currency);

        var getHourPricesResult = await _entsoeHttpClient.GetHourPrices(getHourPricesArgs);

        await CalculateExchangeRates(args.Currency, end, args.Area, getHourPricesResult);

        if (getHourPricesResult.Prices is null)
            throw new NullReferenceException("Prices was not set");
        
        return new GetAveragePricesResult
        {
            Today = getHourPricesResult.Prices.GetAverageOnLastDate(),
            Month = getHourPricesResult.Prices.GetAverageOnLastMonth(),
            Last31Days = getHourPricesResult.Prices.GetAverage(),
            PriceUnit = getHourPricesResult.GetPriceUnit()
        };
    }
}