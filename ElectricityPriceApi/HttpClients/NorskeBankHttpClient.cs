using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Extensions;
using Newtonsoft.Json;

namespace ElectricityPriceApi.HttpClients;

public class NorskeBankHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string Format = "yyyy-MM-dd";

    public NorskeBankHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ExchangeRateResult> GetExchangeRate(ExchangeRateArgs args)
    {
        var area = args.Area;
        var startPeriod = args.PeriodStart.ConvertTimeToUtc(area);
        var endPeriod = args.PeriodEnd.ConvertTimeToUtc(area);

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get, $"https://data.norges-bank.no/api/data/EXR/B.{args.FromCurrency}.{args.ToCurrency}.SP?format=sdmx-json&startPeriod={startPeriod.ToString(Format)}&endPeriod={endPeriod.ToString(Format)}&locale=en");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        httpResponseMessage.EnsureSuccessStatusCode();

        var json = await httpResponseMessage.Content.ReadAsStringAsync();

        var deserializeObject = JsonConvert.DeserializeObject<ExchangeRateJson>(json);

        var observation = float.Parse(deserializeObject.data.dataSets.First().series._0000.observations.Last().Value.First(), CultureInfo.InvariantCulture);

        var unitMultiplier = deserializeObject.data.structure.attributes.series.FirstOrDefault(x => x.id.Equals("UNIT_MULT"))?.values.FirstOrDefault()?.id;

        if (int.TryParse(unitMultiplier, out var unitMultiplierResult))
        {
            if (unitMultiplierResult > 0)
                observation /= (float)(Math.Pow(10,unitMultiplierResult));
        }

        var result = new ExchangeRateResult
        {
            FromCurrency = deserializeObject.data.structure.dimensions.series.First(x => x.keyPosition == 1).values.First().id,
            ToCurrency = deserializeObject.data.structure.dimensions.series.First(x => x.keyPosition == 2).values.First().id,
            ExchangeRate = observation
        };
        return result;
    }
}

public class ExchangeRateArgs
{
    public ExchangeRateArgs(DateTime periodStart, DateTime periodEnd, Area area, string fromCurrency, string toCurrency)
    {
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        Area = area;
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
    }

    public DateTime PeriodStart { get; }
    public DateTime PeriodEnd { get; }
    public Area Area { get; }
    public string FromCurrency { get; }
    public string ToCurrency { get; }
}

public class ExchangeRateResult
{
    public string? FromCurrency { get; set; }
    public string? ToCurrency { get; set; }
    public float ExchangeRate { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class _0000
{
    public List<int> attributes { get; set; }
    public Dictionary<string, List<string>> observations { get; set; }
}

public class Attributes
{
    public List<object> dataset { get; set; }
    public List<Series> series { get; set; }
    public List<object> observation { get; set; }
}

public class Data
{
    public List<DataSet> dataSets { get; set; }
    public Structure structure { get; set; }
}

public class DataSet
{
    public List<Link> links { get; set; }
    public string action { get; set; }
    public Series series { get; set; }
}

public class Descriptions
{
    public string en { get; set; }
}

public class Dimensions
{
    public List<object> dataset { get; set; }
    public List<Series> series { get; set; }
    public List<Observation> observation { get; set; }
}

public class Link
{
    public string href { get; set; }
    public string rel { get; set; }
    public string uri { get; set; }
    public string urn { get; set; }
}

public class Meta
{
    public string id { get; set; }
    public DateTime prepared { get; set; }
    public bool test { get; set; }
    public Sender sender { get; set; }
    public Receiver receiver { get; set; }
    public List<Link> links { get; set; }
}

public class Names
{
    public string en { get; set; }
}

public class Observation
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int keyPosition { get; set; }
    public string role { get; set; }
    public List<Value> values { get; set; }
}

public class Receiver
{
    public string id { get; set; }
}

public class Relationship
{
    public List<string> dimensions { get; set; }
}

public class ExchangeRateJson
{
    public Meta meta { get; set; }
    public Data data { get; set; }
}

public class Sender
{
    public string id { get; set; }
}

public class Series
{
    [JsonProperty("0:0:0:0")]
    public _0000 _0000 { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int keyPosition { get; set; }
    public object role { get; set; }
    public List<Value> values { get; set; }
    public Relationship relationship { get; set; }
}

public class Structure
{
    public List<Link> links { get; set; }
    public string name { get; set; }
    public Names names { get; set; }
    public string description { get; set; }
    public Descriptions descriptions { get; set; }
    public Dimensions dimensions { get; set; }
    public Attributes attributes { get; set; }
}

public class Value
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
}

