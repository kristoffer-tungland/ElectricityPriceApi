using System;
using ElectricityPriceApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

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