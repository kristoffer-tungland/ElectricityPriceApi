using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public class GetHourResult
{
    public int Hour { get; set; }
    public int Score { get; set; }
    public float Price { get; set; }
    public string? PriceUnit { get; set; }

    // Used for XML serializing
    public GetHourResult()
    {

    }

    public GetHourResult(HourPriceScore hourPriceScore, string? priceUnit)
    {
        Hour = hourPriceScore.Time.Hour;
        Score = hourPriceScore.Score;
        Price = hourPriceScore.Price;
        PriceUnit = priceUnit;
    }
}