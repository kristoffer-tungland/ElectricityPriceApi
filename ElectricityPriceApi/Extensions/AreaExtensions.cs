using System;
using System.Globalization;
using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Extensions;

public static class AreaExtensions
{
    public static TimeZoneInfo ToTimeZone(this Area area)
    {
        var timeZoneId = area switch
        {
            Area.No1 or Area.No2 or Area.No3 or Area.No4 or Area.No5 => "W. Europe Standard Time",
            _ => throw new ArgumentOutOfRangeException(nameof(area), area, null)
        };

        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }

    public static int GetCurrentLocalHour(this Area area)
    {
        var utcNow = DateTime.UtcNow;
        var dateTime = DateTime.Parse(utcNow.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        var timeZone = area.ToTimeZone();
        var dateTimeOffset = new DateTimeOffset(dateTime, -timeZone.GetUtcOffset(dateTime));
        return dateTimeOffset.UtcDateTime.Hour;
    }
}