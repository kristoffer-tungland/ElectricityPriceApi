using System.Globalization;
using System.Net.Http;
using ElectricityPriceApi.Exceptions;

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

        if (deserializeObject is null)
            throw new InvalidOperationException();

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

