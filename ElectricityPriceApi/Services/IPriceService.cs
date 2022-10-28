using System.Threading.Tasks;
using ElectricityPriceApi.Services.Prices;

namespace ElectricityPriceApi.Services;

public interface IPriceService
{
    Task<GetHourPricesResult> GetHourPrices(GetHourPricesArgs args);
}