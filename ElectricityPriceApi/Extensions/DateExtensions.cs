using System;

namespace ElectricityPriceApi.Extensions;

public static class DateExtensions
{
    public static DateTime Today => DateTime.Now.SetHour(DateTime.Now.Hour);
    public static DateTime Tomorrow => Today.AddDays(1);

    public static DateTime SetHour(this DateTime dateTime, int hour)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0);
    }
}