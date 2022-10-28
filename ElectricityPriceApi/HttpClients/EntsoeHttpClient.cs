using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Exceptions;
using ElectricityPriceApi.Extensions;
using ElectricityPriceApi.Models;
using ElectricityPriceApi.Services.Prices;
using ElectricityPriceApi.XMLSchemas;
using Microsoft.Extensions.Options;

namespace ElectricityPriceApi.HttpClients;

public class EntsoeHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _apiKey;
    private readonly Regex _removeZRegex = new(@"((?<=T\d{2}:\d{2}))Z");
    private const string Format = "yyyyMMddHHmm";

    public EntsoeHttpClient(IHttpClientFactory httpClientFactory,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = myConfigurationSecrets.Value?.EntsoeApiKey;
    }

    public async Task<GetHourPricesResult> GetHourPrices(GetHourPricesArgs args)
    {
        var area = args.Area;
        var inDomain = area.ConvertToDomain();
        var outDomain = area.ConvertToDomain();

        var periodStart = args.PeriodStart.ConvertTimeToUtc(area);
        var periodEnd = args.PeriodEnd.ConvertTimeToUtc(area);

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get, $"https://transparency.entsoe.eu/api?documentType=A44&in_Domain={inDomain}&out_Domain={outDomain}&periodStart={periodStart.ToString(Format)}&periodEnd={periodEnd.ToString(Format)}&securityToken={_apiKey}");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new FailedToGetDayAheadPricesException(httpResponseMessage);
        }

        var xml = await httpResponseMessage.Content.ReadAsStringAsync();

        Type? serializeToType = null;

        if (xml.Contains("</Acknowledgement_MarketDocument>"))
        {
            serializeToType = typeof(AcknowledgementMarketDocument);
        }
        else
        {
            xml = RemoveZInDates(xml);
            serializeToType = typeof(PublicationMarketDocument);
        }

        var serializer = new XmlSerializer(serializeToType);

        using var reader = new StringReader(xml);
        var objectDeserialize = serializer.Deserialize(reader);

        return objectDeserialize switch
        {
            PublicationMarketDocument publicationMarketDocument => new GetHourPricesResult
            {
                Prices = publicationMarketDocument.TimeSeries.SelectMany(x => Flatten(x, args.Area)).ToList(), 
                CurrencyUnitName = publicationMarketDocument.TimeSeries.First().CurrencyUnitName, 
                PriceMeasureUnitName = "kWh"
            },
            AcknowledgementMarketDocument acknowledgementMarketDocument => throw new DayAheadPricesNotFoundException(acknowledgementMarketDocument),
            _ => throw new InvalidOperationException()
        };
    }

    private string RemoveZInDates(string xml)
    {
        return _removeZRegex.Replace(xml, ":00Z");
    }

    private static IEnumerable<HourPrice> Flatten(TimeSeries timeSeries, Area area)
    {
        return timeSeries.Period.Point.Select(x => ConvertPointToHourPrice(x, timeSeries, area)).ToList();
    }

    private static HourPrice ConvertPointToHourPrice(Point point, TimeSeries timeSeries, Area area)
    {
        var timeIntervalStart = timeSeries.Period.TimeInterval.Start;

        var startTime = timeIntervalStart.AddHours(point.Position - 1);
        var utcTime = DateTime.Parse(startTime.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        var localTime = utcTime.ConvertTimeFromUtc(area);

        return new HourPrice
        {
            Price = (float)point.PriceAmount / 1000,
            Time = localTime
        };
    }
}