using System;
using System.Net.Http;

namespace ElectricityPriceApi.Exceptions;

public class FailedToGetDayAheadPricesException : Exception
{
    public FailedToGetDayAheadPricesException(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage.ReasonPhrase)
    {
        StatusCode = httpResponseMessage.StatusCode.ToString();
    }

    public string? StatusCode { get; }
}