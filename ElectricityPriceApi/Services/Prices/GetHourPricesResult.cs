using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesResult
{
    public List<HourPrice>? Prices { get; set; }
    public string? CurrencyUnitName { get; set; }
    public string? PriceMeasureUnitName { get; set; }

    public string? GetPriceUnit()
    {
        return $"{CurrencyUnitName}/{PriceMeasureUnitName}";
    }
}