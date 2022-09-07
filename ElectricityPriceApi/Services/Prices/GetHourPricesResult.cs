using System.Collections.Generic;
using ElectricityPriceApi.Models;

namespace ElectricityPriceApi.Services.Prices;

public class GetHourPricesResult
{
    public List<HourPrice>? Prices { get; set; }
}