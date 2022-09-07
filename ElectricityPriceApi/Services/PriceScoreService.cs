﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Services.Prices;
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

    public async Task<int> GetScore(DateTimeOffset dateTime, Area area)
    {
        var periodStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        var periodEnd = periodStart.AddHours(24);
        
        var args = new GetHourPricesArgs(area, periodStart, periodEnd);
        try
        {
            var hourPricesResult = await _priceService.GetHourPrices(args);
            var hour = dateTime.ToLocalTime().Hour;
            return GetScore(hour, (hourPricesResult.Prices ?? throw new InvalidOperationException()).ToDictionary(x => x.StartTime.Hour, x => x.PriceAmount));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static int GetScore(int hour, Dictionary<int, float> hourPrices)
    {
        if (hourPrices.TryGetValue(hour, out _))
        {
            var orderedList = hourPrices.OrderBy(x => x.Value).ToList();

            var pair = orderedList.First(x => x.Key == hour);

            var index = orderedList.IndexOf(pair);

            return index + 1;
        }

        throw new Exception($"Could not get score from hour {hour}");

    }

    public async Task<int> GetHour(DateTime dateTime, int score, Area area)
    {
        var periodStart = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        var periodEnd = periodStart.AddHours(24);

        var args = new GetHourPricesArgs(area, periodStart, periodEnd);
        try
        {
            var hourPricesResult = await _priceService.GetHourPrices(args);
            return GetHour(score, (hourPricesResult.Prices ?? throw new InvalidOperationException()).ToDictionary(x => x.StartTime.Hour, x => x.PriceAmount));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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