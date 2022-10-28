using System.Globalization;
using ElectricityPriceApi.Enums;

namespace ElectricPrice.Tests;

public class TimeZoneTests
{
    [Fact]
    public void ConvertToLocalTime()
    {
        var dateTimeOffset = DateTimeOffset.UtcNow;
        var timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.DateTime, timeZone);
    }

    [Fact]
    public void CreateLocalTime()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        var dateTime = DateTime.Parse("2022-09-06 07:56", CultureInfo.InvariantCulture);
        var localTime = new DateTimeOffset(dateTime, timeZone.GetUtcOffset(dateTime));
        var utcTime = localTime.ToUniversalTime();
    }
}