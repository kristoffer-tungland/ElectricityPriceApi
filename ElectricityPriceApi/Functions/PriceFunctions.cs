using ElectricityPriceApi.Services;
using ElectricityPriceApi.Services.Prices;

namespace ElectricityPriceApi.Functions;

public class PriceFunctions
{
    private readonly IPriceService _priceService;

    public PriceFunctions(IPriceService priceService)
    {
        _priceService = priceService;
    }

    [FunctionName("Price")]
    [OpenApiOperation("RunPrice", "name", Description = "Get prices within a date range")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("fromDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
    [OpenApiParameter("toDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetHourPricesResult), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunPrice(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        if (!req.TryGetDateParameter("fromDate", out var fromDate))
            return new BadRequestErrorMessageResult("Date was not on correct format");

        if (!req.TryGetDateParameter("toDate", out var toDate))
            return new BadRequestErrorMessageResult("Date was not on correct format");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var args = new GetHourPricesArgs(area, fromDate, toDate, currency);
            var result = await _priceService.GetHourPrices(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get price");
            return new ExceptionResult(e, true);
        }
    }

    [FunctionName("AveragePrices")]
    [OpenApiOperation("RunAveragePrices", "name", Description = "Get the average price for today, this month and last 31 days")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetAveragePricesResult), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunAveragePrices(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");
        
        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var args = new GetAveragePricesArgs(area, currency);
            var result = await _priceService.GetAveragePrices(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get price");
            return new ExceptionResult(e, true);
        }
    }
}