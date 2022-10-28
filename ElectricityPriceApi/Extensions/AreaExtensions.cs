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

    public static DateTime LocalTimeNow(this Area area)
    {
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ConvertTimeFromUtc(area);

        return localTime;
    }

    public static int CurrentLocalHour(this Area area)
    {
        var utcNow = DateTime.UtcNow;
        var localTime = utcNow.ConvertTimeFromUtc(area);

        return localTime.Hour;
    }
}