using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult(int scoreNow, int hourNow, List<HourPriceScore> prices)
    {
        ScoreNow = scoreNow;
        HourNow = hourNow;
        Prices = prices;

        foreach (var hourPriceScore in prices)
        {
            HourScores.Add($"ScoreOfHour{hourPriceScore.Time.Hour}", hourPriceScore.Score);
            HourPrices.Add($"PriceOfHour{hourPriceScore.Time.Hour}", hourPriceScore.Price);
        }
    }

    public int ScoreNow { get; }
    public int HourNow { get; }
    
    public Dictionary<string, int> HourScores { get; } = new();
    public Dictionary<string, float> HourPrices { get; } = new();

    public List<HourPriceScore> Prices { get; }
}