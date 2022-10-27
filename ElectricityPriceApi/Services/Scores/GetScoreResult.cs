using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult()
    {
        
    }

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
    
    public int ScoreNow { get; set; }
    public int HourNow { get; set; }
    public float PriceNow { get; set; }
    public string? PriceUnit { get; set; }
    public SerializableDictionary<string, int> HourScores { get; set; } = new();
    public SerializableDictionary<string, float> HourPrices { get; set; } = new();
}