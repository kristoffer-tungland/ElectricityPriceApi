namespace ElectricityPriceApi.Services.Prices;

public interface IPriceService
{
    Task<GetHourPricesResult> GetHourPrices(GetHourPricesArgs args);
    Task<GetAveragePricesResult?> GetAveragePrices(GetAveragePricesArgs args);
}