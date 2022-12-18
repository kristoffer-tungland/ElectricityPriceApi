using ElectricityPriceApi.Models;
using System.Globalization;

namespace ElectricityPriceApi.Extensions;

public static class HourPricesExtensions
{
    public static float GetAverageOnLastDay(this List<HourPrice> prices)
    {
        var date = prices.Last().Time.Date;

        var lastDay = prices.Where(x => x.Time.Date.Equals(date));

        return lastDay.Select(x => x.Price).Average();
    }
    public static float GetAverageOnLastWeek(this List<HourPrice> prices)
    {
        var date = prices.Last().Time.Date;

        var weekNumber = ISOWeek.GetWeekOfYear(date);

        var lastWeek = prices.Where(x => x.Time.Year.Equals(date.Year) && ISOWeek.GetWeekOfYear(x.Time).Equals(weekNumber));

        return lastWeek.Select(x => x.Price).Average();
    }
    
    public static float GetAverageOnLastMonth(this List<HourPrice> prices)
    {
        var date = prices.Last().Time.Date;

        var lastMonth = prices.Where(x => x.Time.Year.Equals(date.Year) && x.Time.Month.Equals(date.Month));

        return lastMonth.Select(x => x.Price).Average();
    }

    public static float GetAverageOnLast7Days(this List<HourPrice> prices)
    {
        var last7Days = prices.TakeLast(7 * 24);

        return last7Days.Select(x => x.Price).Average();
    }

    public static float GetAverage(this List<HourPrice> prices)
    {
        return prices.Select(x => x.Price).Average();
    }
}