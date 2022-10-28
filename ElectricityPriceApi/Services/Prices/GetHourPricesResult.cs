using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesResult
{
    public string? CurrencyUnitName { get; set; }
    public string? PriceMeasureUnitName { get; set; }

    public List<HourPrice>? Prices { get; set; }
    

    public string GetPriceUnit()
    {
        return $"{CurrencyUnitName}/{PriceMeasureUnitName}";
    }
}