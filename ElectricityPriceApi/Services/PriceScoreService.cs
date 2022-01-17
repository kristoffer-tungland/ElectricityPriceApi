using System;
using System.Threading.Tasks;
using ElectricityPriceApi.Models;
using Microsoft.Extensions.Options;

namespace ElectricityPriceApi.Services;

public class PriceScoreService
{
    private readonly MyConfiguration _myConfiguration;
    private readonly MyConfigurationSecrets _myConfigurationSecrets;

    public PriceScoreService(IOptions<MyConfiguration> myConfiguration,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _myConfiguration = myConfiguration.Value;
        _myConfigurationSecrets = myConfigurationSecrets.Value;
    }

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