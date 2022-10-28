using ElectricityPriceApi.XMLSchemas;

namespace ElectricityPriceApi.Exceptions;

public class DayAheadPricesNotFoundException : Exception
{
    public DayAheadPricesNotFoundException(AcknowledgementMarketDocument acknowledgementMarketDocument) : base(acknowledgementMarketDocument.Reason?.Text)
    {
        StatusCode = acknowledgementMarketDocument.Reason?.Code;
    }

    public string? StatusCode { get; set; }
}