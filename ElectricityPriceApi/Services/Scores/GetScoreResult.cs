using ElectricityPriceApi.Services.Prices;

namespace ElectricityPriceApi.Services.Scores;

public class GetScoreResult
{
    public GetScoreResult(int score, GetHourPricesResult hourPricesResult)
    {
        Score = score;
        HourPricesResult = hourPricesResult;
    }

    public int Score { get; }
    public GetHourPricesResult HourPricesResult { get; }
}