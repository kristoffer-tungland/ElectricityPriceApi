using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    // Used for XML serializing
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
            if (!HourScores.ContainsKey($"ScoreOfHour{priceScore.Time.Hour}"))
                HourScores.Add($"ScoreOfHour{priceScore.Time.Hour}", priceScore.Score);

            if (!HourPrices.ContainsKey($"PriceOfHour{priceScore.Time.Hour}"))
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