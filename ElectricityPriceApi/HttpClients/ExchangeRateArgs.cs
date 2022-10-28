namespace ElectricityPriceApi.HttpClients;

public class ExchangeRateArgs
{
    public ExchangeRateArgs(DateTime periodStart, DateTime periodEnd, Area area, string fromCurrency, string toCurrency)
    {
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        Area = area;
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
    }

    public DateTime PeriodStart { get; }
    public DateTime PeriodEnd { get; }
    public Area Area { get; }
    public string FromCurrency { get; }
    public string ToCurrency { get; }
}