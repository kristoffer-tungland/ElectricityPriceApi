namespace ElectricityPriceApi.HttpClients;

public class ExchangeRateResult
{
    public string? FromCurrency { get; set; }
    public string? ToCurrency { get; set; }
    public float ExchangeRate { get; set; }
}