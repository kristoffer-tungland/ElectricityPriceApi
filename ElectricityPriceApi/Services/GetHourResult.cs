using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services;

public class GetHourResult
{
    public int Hour { get; }
    public int Score { get; }
    public float Price { get; }
    public string? PriceUnit { get; set; }

    public GetHourResult(HourPriceScore hourPriceScore, string? priceUnit)
    {
        Hour = hourPriceScore.Time.Hour;
        Score = hourPriceScore.Score;
        Price = hourPriceScore.Price;
        PriceUnit = priceUnit;
    }
}