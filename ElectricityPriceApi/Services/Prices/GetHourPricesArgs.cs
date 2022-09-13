using System;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesArgs
{
    public GetHourPricesArgs(Area area, DateTime periodStart, DateTime periodEnd)
    {
        Area = area;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public Area Area { get; }
    public DateTime PeriodStart { get; }
    public DateTime PeriodEnd { get; }
}