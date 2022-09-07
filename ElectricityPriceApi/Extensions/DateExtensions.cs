using System;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Extensions;

public static class DateExtensions
{
    public static DateTime Today => DateTime.UtcNow;
    public static DateTime Tomorrow => Today.AddDays(1);

    public static DateTime SetHour(this DateTime dateTime, int hour)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0);
    }

    public static DateTimeOffset ToLocalTime(this DateTime dateTime, Area area)
    {
        var timeZone = area.ToTimeZone();
        return new DateTimeOffset(dateTime, timeZone.GetUtcOffset(dateTime));
    }
}