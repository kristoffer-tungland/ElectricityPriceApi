using ElectricityPriceApi.Exceptions;

namespace ElectricityPriceApi.Extensions;

public static class ExceptionExtensions
{
    public static IActionResult? HandleException(this Exception e)
    {
        if (e is DayAheadPricesNotFoundException dayAheadPricesNotFoundException)
        {
            return new NotFoundObjectResult(dayAheadPricesNotFoundException.Message);
        }

        return null;
    }
}