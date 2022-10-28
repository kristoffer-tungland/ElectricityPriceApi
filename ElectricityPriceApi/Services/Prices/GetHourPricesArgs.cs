namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesArgs
{
    public GetHourPricesArgs(Area area, DateTime periodStart, DateTime periodEnd, string currency)
    {
        Area = area;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        Currency = currency;
    }

    public Area Area { get; }
    public DateTime PeriodStart { get; }
    public DateTime PeriodEnd { get; }
    public string Currency { get; }
}