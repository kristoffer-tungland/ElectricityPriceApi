using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Extensions;

public static class HourPricesExtensions
{
    public static float GetAverageOnLastDate(this List<HourPrice> prices)
    {
        var date = prices.Last().Time.Date;

        var pricesThisDate = prices.Where(x => x.Time.Date.Equals(date));

        return pricesThisDate.Select(x => x.Price).Average();
    }

    public static float GetAverageOnLastMonth(this List<HourPrice> prices)
    {
        var date = prices.Last().Time.Date;

        var pricesThisDate = prices.Where(x => x.Time.Year.Equals(date.Year) && x.Time.Month.Equals(date.Month));

        return pricesThisDate.Select(x => x.Price).Average();
    }

    public static float GetAverage(this List<HourPrice> prices)
    {
        return prices.Select(x => x.Price).Average();
    }
}