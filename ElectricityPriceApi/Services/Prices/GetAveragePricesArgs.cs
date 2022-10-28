using ElectricityPriceApi.Enums;

namespace ElectricityPriceApi.Services.Prices;

public class GetAveragePricesArgs
{
    public Area Area { get; }
    public string Currency { get; }

    public GetAveragePricesArgs(Area area, string currency)
    {
        Area = area;
        Currency = currency;
    }
}