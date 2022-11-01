namespace ElectricityPriceApi.Services.Prices;

public class GetAveragePricesResult
{
    public float Today { get; set; }
    public float Week { get; set; }
    public float Month { get; set; }
    public float Last7Days { get; set; }
    public float Last31Days { get; set; }
    public string? PriceUnit { get; set; }
}