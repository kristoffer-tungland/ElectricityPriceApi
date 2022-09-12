using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult(int score, int hour, List<HourPriceScore> prices)
    {
        Score = score;
        Hour = hour;
        Prices = prices;
    }

    public int Score { get; }
    public int Hour { get; }
    public List<HourPriceScore> Prices { get; }
}