using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ElectricityPriceApi.HttpClients;

public class PriceHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _apiKey;

    public PriceHttpClient(IHttpClientFactory httpClientFactory,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _httpClientFactory = httpClientFactory;
        _apiKey = myConfigurationSecrets.Value?.PriceApiKey;
    }

    public async Task<string?> GetAspNetCoreDocsBranchesAsync()
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get, $"https://norway-power.ffail.win?key={_apiKey}");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        httpResponseMessage.EnsureSuccessStatusCode();

        return await httpResponseMessage.Content.ReadAsStringAsync();

        //await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

        //return await JsonSerializer.DeserializeAsync<IEnumerable<ElectricalPrice>>(contentStream);
    }

    public async Task<IEnumerable<Prices>?> GetDesignData()
    {
        await Task.Delay(300);
        var contentStream = "[\r\n    {\r\n        \"Date\": \"2022-01-21\",\r\n        \"Price\": {\r\n            \"2022-01-21T00:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2673,\r\n                \"valid_from\": \"2022-01-21T00:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T01:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T01:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2605,\r\n                \"valid_from\": \"2022-01-21T01:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T02:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T02:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2668,\r\n                \"valid_from\": \"2022-01-21T02:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T03:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T03:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2717,\r\n                \"valid_from\": \"2022-01-21T03:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T04:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T04:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2746,\r\n                \"valid_from\": \"2022-01-21T04:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T05:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T05:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.3039,\r\n                \"valid_from\": \"2022-01-21T05:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T06:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T06:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.3175,\r\n                \"valid_from\": \"2022-01-21T06:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T07:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T07:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4068,\r\n                \"valid_from\": \"2022-01-21T07:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T08:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T08:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4358,\r\n                \"valid_from\": \"2022-01-21T08:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T09:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T09:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4337,\r\n                \"valid_from\": \"2022-01-21T09:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T10:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T10:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4324,\r\n                \"valid_from\": \"2022-01-21T10:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T11:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T11:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4178,\r\n                \"valid_from\": \"2022-01-21T11:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T12:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T12:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4047,\r\n                \"valid_from\": \"2022-01-21T12:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T13:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T13:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4356,\r\n                \"valid_from\": \"2022-01-21T13:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T14:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T14:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4182,\r\n                \"valid_from\": \"2022-01-21T14:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T15:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T15:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4285,\r\n                \"valid_from\": \"2022-01-21T15:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T16:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T16:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.4477,\r\n                \"valid_from\": \"2022-01-21T16:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T17:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T17:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.474,\r\n                \"valid_from\": \"2022-01-21T17:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T18:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T18:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.46,\r\n                \"valid_from\": \"2022-01-21T18:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T19:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T19:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.404,\r\n                \"valid_from\": \"2022-01-21T19:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T20:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T20:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.3592,\r\n                \"valid_from\": \"2022-01-21T20:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T21:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T21:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.3333,\r\n                \"valid_from\": \"2022-01-21T21:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T22:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T22:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.3275,\r\n                \"valid_from\": \"2022-01-21T22:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-21T23:00:00+01:00\"\r\n            },\r\n            \"2022-01-21T23:00:00+01:00\": {\r\n                \"NOK_per_kWh\": 1.2677,\r\n                \"valid_from\": \"2022-01-21T23:00:00+01:00\",\r\n                \"valid_to\": \"2022-01-22T00:00:00+01:00\"\r\n            }\r\n        }\r\n    }\r\n]";

        return JsonConvert.DeserializeObject<IEnumerable<Prices>>(contentStream);
    }
}


public class Prices
{
    [JsonProperty("Date")]
    public DateTime Date { get; set; }

    [JsonProperty("Price")]
    public Dictionary<DateTimeOffset, Price> Price { get; set; }
}

public class Price
{
    [JsonProperty("NOK_per_kWh")]
    public double NokPerKWh { get; set; }

    [JsonProperty("valid_from")]
    public DateTimeOffset ValidFrom { get; set; }

    [JsonProperty("valid_to")]
    public DateTimeOffset ValidTo { get; set; }
}