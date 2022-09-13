using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult(int scoreNow, int hour, List<HourPriceScore> prices)
    {
        ScoreNow = scoreNow;
        Hour = hour;
        Prices = prices;
    }

    public int ScoreNow { get; }
    public int Hour { get; }
    public List<HourPriceScore> Prices { get; }
}