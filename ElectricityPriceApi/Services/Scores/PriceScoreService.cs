using ElectricityPriceApi.Models;
using ElectricityPriceApi.Services.Prices;

namespace ElectricityPriceApi.Services.Scores;

internal class PriceScoreService : IPriceScoreService
{
    private readonly IPriceService _priceService;

    public PriceScoreService(IPriceService priceService)
    {
        _priceService = priceService;
    }

    public async Task<GetScoreResult> GetScore(GetScoreArgs args)
    {
        var area = args.Area;
        var localTime = args.LocalTime;
        var periodStart = new DateTime(localTime.Year, localTime.Month, localTime.Day);
        var periodEnd = periodStart.AddHours(24);

        var getHourPricesArgs = new GetHourPricesArgs(area, periodStart, periodEnd, args.Currency);

        var hourPricesResult = await _priceService.GetHourPrices(getHourPricesArgs);

        if (hourPricesResult.Prices is null)
            throw new NullReferenceException(nameof(hourPricesResult));

        var pricesWithScore = CalculateScoreOnPrices(hourPricesResult.Prices);

        var hour = localTime.ToLocalTime().Hour;

        var hourPriceScore = pricesWithScore.First(x => x.Time.Hour == hour);

        return new GetScoreResult(hourPriceScore, pricesWithScore, hourPricesResult.GetPriceUnit());
    }

    public List<HourPriceScore> CalculateScoreOnPrices(List<HourPrice> prices)
    {
        var result = new List<HourPriceScore>();
        var orderedPrices = prices.OrderBy(x => x.Price).ToList();

        foreach (var hourPriceScore in prices.Select(hourPrice => new HourPriceScore(hourPrice)))
        {
            result.Add(hourPriceScore);
            var score = orderedPrices.First(x => x.Time.Equals(hourPriceScore.Time));
            hourPriceScore.Score = orderedPrices.IndexOf(score) + 1;
        }

        return result;
    }

    public async Task<GetHourResult> GetHour(GetHourArgs args)
    {
        var localTime = args.LocalTime;
        var area = args.Area;
        var score = args.Score;

        var periodStart = new DateTime(localTime.Year, localTime.Month, localTime.Day);
        var periodEnd = periodStart.AddHours(24);

        var getHourPricesArgs = new GetHourPricesArgs(area, periodStart, periodEnd, args.Currency);

        var hourPricesResult = await _priceService.GetHourPrices(getHourPricesArgs);

        if (hourPricesResult.Prices is null)
            throw new NullReferenceException(nameof(hourPricesResult));

        var pricesWithScore = CalculateScoreOnPrices(hourPricesResult.Prices);

        var hourPriceScore = pricesWithScore.First(x => x.Score == score);

        return new GetHourResult(hourPriceScore, hourPricesResult.GetPriceUnit());
    }
}