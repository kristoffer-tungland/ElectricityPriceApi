namespace ElectricityPriceApi.Services.Prices;

public class GetAveragePricesResult
{
    public float Today { get; set; }
    public float Month { get; set; }
    public float Last31Days { get; set; }
}