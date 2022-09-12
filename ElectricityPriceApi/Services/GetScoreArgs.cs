using System;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Services;

public class GetScoreArgs
{
    public DateTimeOffset LocalTime { get; }
    public Area Area { get; }

    public GetScoreArgs(DateTimeOffset localTime, Area area)
    {
        LocalTime = localTime;
        Area = area;
    }
}