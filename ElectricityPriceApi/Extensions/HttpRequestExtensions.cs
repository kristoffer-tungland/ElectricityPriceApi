using System.Globalization;

namespace ElectricityPriceApi.Extensions;

public static class HttpRequestExtensions
{
    public static bool FormatParameterEquals(this HttpRequest request, Format format)
    {
        return Enum.TryParse(request.Query["format"], true, out Format outFormat) && outFormat.Equals(format);
    }

    public static string GetCurrencyParameterOrDefault(this HttpRequest request)
    {
        var currency = request.Query["currency"];
        currency = string.IsNullOrEmpty(currency) ? "EUR" : currency.ToString().ToUpper();
        return currency;
    }

    public static bool TryGetAreaParameter(this HttpRequest request, out Area area)
    {
        return Enum.TryParse(request.Query["area"], true, out area);
    }

    public static bool TryGetHourParameter(this HttpRequest request, out int hour)
    {
        return int.TryParse(request.Query["hour"], out hour);
    }

    public static bool TryGetDateParameter(this HttpRequest request, out DateTime date)
    {
        return request.TryGetDateParameter("date", out date);
    }

    public static bool TryGetDateParameter(this HttpRequest request, string dateParameterName, out DateTime date)
    {
        return DateTime.TryParse(request.Query[dateParameterName], CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    public static void SetAcceptHeadersIfFormatSetToXml(this HttpRequest request)
    {
        if (request.FormatParameterEquals(Format.xml))
            request.Headers.Accept = "application/xml";
    }

    public static bool TryGetScoreParameter(this HttpRequest request, out int score)
    {
        return int.TryParse(request.Query["score"], out score);
    }
}