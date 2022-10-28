using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Scores;

public interface IPriceScoreService
{
    Task<GetScoreResult> GetScore(GetScoreArgs args);
    List<HourPriceScore> CalculateScoreOnPrices(List<HourPrice> prices);
    Task<GetHourResult> GetHour(GetHourArgs args);
}