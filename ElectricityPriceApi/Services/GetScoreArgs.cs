using System;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Services;

public class GetScoreArgs
{
    public DateTimeOffset LocalTime { get; }
    public Area Area { get; }
    public string Currency { get; set; }

    public GetScoreArgs(DateTimeOffset localTime, Area area, string currency)
    {
        LocalTime = localTime;
        Area = area;
        Currency = currency;
    }
}