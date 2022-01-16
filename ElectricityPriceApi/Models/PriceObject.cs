using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectricityPriceApi.Models;

public static class PriceObject
{
    public static Dictionary<int, float> Prices { get; } = CreateDesignData();

    private static Dictionary<int, float> CreateDesignData()
    {
        var random = new Random();
        var result = new Dictionary<int, float>();

        for (var i = 0; i <= 23; i++)
        {
            var randomNumber = random.Next(0, 24);

            result.Add(i, randomNumber);
        }

        return result;
    }

    public static int GetScore(int hour)
    {
        if (Prices.TryGetValue(hour, out var value))
        {
            var orderedList = Prices.OrderBy(x => x.Value).ToList();

            var pair = orderedList.First(x => x.Key == hour);

            var index = orderedList.IndexOf(pair);

            return index + 1;
        }

        throw new Exception($"Could not get score from hour {hour}");

    }

    public static int GetHour(DateTime dateTime, int score)
    {
        var key = score - 1;

        var orderedList = Prices.OrderBy(x => x.Value).ToList();

        if (orderedList.Count >= key)
        {
            var pair = orderedList[key];
                
            return pair.Key;
        }

        throw new Exception($"Could not get hour from score {score}");
    }
}