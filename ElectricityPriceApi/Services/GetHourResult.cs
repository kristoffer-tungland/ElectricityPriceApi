using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services;

public class GetHourResult
{
    public int Hour { get; }
    public int Score { get; }
    public List<HourPriceScore> Prices { get; }

    public GetHourResult(int hour, int score, List<HourPriceScore> prices)
    {
        Hour = hour;
        Score = score;
        Prices = prices;
    }
}