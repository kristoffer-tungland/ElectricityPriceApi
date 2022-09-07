using System;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesArgs
{
    public GetHourPricesArgs(Area area, DateTimeOffset periodStart, DateTimeOffset periodEnd)
    {
        Area = area;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public Area Area { get; }
    public DateTimeOffset PeriodStart { get; }
    public DateTimeOffset PeriodEnd { get; }
}