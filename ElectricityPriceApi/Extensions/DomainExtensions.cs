namespace ElectricityPriceApi.Extensions;

public static class DomainExtensions
{
    public static string ConvertToDomain(this Area area)
    {
        // Link to area codes https://transparency.entsoe.eu/content/static_content/Static%20content/web%20api/Guide.html#_areas
        return area switch
        {
            Area.No1 => "10YNO-1--------2",
            Area.No2 => "10YNO-2--------T",
            Area.No3 => "10YNO-3--------J",
            Area.No4 => "10YNO-4--------9",
            Area.No5 => "10Y1001A1001A48H",
            _ => throw new ArgumentOutOfRangeException(nameof(area), area, null)
        };
    }
}