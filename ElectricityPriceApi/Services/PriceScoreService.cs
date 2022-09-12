using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Extensions;
using ElectricityPriceApi.Models;
using ElectricityPriceApi.Services.Prices;
using ElectricityPriceApi.Services.Scores;
using Microsoft.Extensions.Options;

namespace ElectricityPriceApi.Services;

public class PriceScoreService
{
    private readonly IPriceService _priceService;

    public PriceScoreService(IOptions<MyConfiguration> myConfiguration,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets, IPriceService priceService)
    {
        _priceService = priceService;
    }

    public async Task<GetScoreResult> GetScore(GetScoreArgs args)
    {
        var area = args.Area;
        var timeZone = area.ToTimeZone();
        var localTime = args.LocalTime;
        var periodStart = new DateTimeOffset(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0, timeZone.GetUtcOffset(localTime));
        var periodEnd = periodStart.AddHours(24);

        var getHourPricesArgs = new GetHourPricesArgs(area, periodStart, periodEnd);

        var hourPricesResult = await _priceService.GetHourPrices(getHourPricesArgs);

        if (hourPricesResult.Prices is null)
            throw new NullReferenceException(nameof(hourPricesResult));

        var pricesWithScore = CalculateScoreOnPrices(hourPricesResult.Prices);

        var hour = localTime.ToLocalTime().Hour;

        var hourPriceScore = pricesWithScore.First(x => x.Time.Hour == hour);
        var score = hourPriceScore.Score;

        return new GetScoreResult(score, hour, pricesWithScore);
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

    public async Task<GetHourResult> GetHour(DateTime dateTime, int score, Area area)
    {
        var periodStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        var periodEnd = periodStart.AddHours(24);

        var args = new GetHourPricesArgs(area, periodStart, periodEnd);

        var hourPricesResult = await _priceService.GetHourPrices(args);

        if (hourPricesResult.Prices is null)
            throw new NullReferenceException(nameof(hourPricesResult));

        var pricesWithScore = CalculateScoreOnPrices(hourPricesResult.Prices);

        var hourPriceScore = pricesWithScore.First(x => x.Score == score);

        var hour = hourPriceScore.Time.ToLocalTime().Hour;

        return new GetHourResult(hour, score, pricesWithScore);
    }

    public static int GetHour(int score, Dictionary<int, float> hourPrices)
    {
        var key = score - 1;

        var orderedList = hourPrices.OrderBy(x => x.Value).ToList();

        if (orderedList.Count >= key)
        {
            var pair = orderedList[key];

            return pair.Key;
        }

        throw new Exception($"Could not get hour from score {score}");
    }
}