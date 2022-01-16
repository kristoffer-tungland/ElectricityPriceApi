using System;
using System.Threading.Tasks;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services;

public class PriceScoreService
{
    public async Task<int> GetScore(DateTime dateTime)
    {
        await Task.Delay(0);

        return PriceObject.GetScore(dateTime.Hour);
    }

    public async Task<int> GetHour(DateTime date, int score)
    {
        await Task.Delay(0);

        return PriceObject.GetHour(date, score);
    }
}