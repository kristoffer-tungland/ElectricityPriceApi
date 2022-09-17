using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult(HourPriceScore hourPriceScore, List<HourPriceScore> pricesWithScore, string? priceUnit)
    {
        ScoreNow = hourPriceScore.Score;
        HourNow = hourPriceScore.Time.Hour;
        PriceNow = hourPriceScore.Price;
        PriceUnit = priceUnit;

        foreach (var priceScore in pricesWithScore)
        {
            HourScores.Add($"ScoreOfHour{priceScore.Time.Hour}", priceScore.Score);
            HourPrices.Add($"PriceOfHour{priceScore.Time.Hour}", priceScore.Price);
        }
    }
    
    public int ScoreNow { get; }
    public int HourNow { get; }
    public float PriceNow { get; set; }
    public string? PriceUnit { get; set; }
    public Dictionary<string, int> HourScores { get; } = new();
    public Dictionary<string, float> HourPrices { get; } = new();
}