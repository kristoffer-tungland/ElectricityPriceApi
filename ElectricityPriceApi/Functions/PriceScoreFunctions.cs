using ElectricityPriceApi.Examples;
using ElectricityPriceApi.Services.Scores;

namespace ElectricityPriceApi.Functions;

public class PriceScoreFunctions
{
    private readonly IPriceScoreService _priceScoreService;

    public PriceScoreFunctions(IPriceScoreService priceScoreService)
    {
        _priceScoreService = priceScoreService;
    }

    [FunctionName("PriceScore")]
    [OpenApiOperation("RunPriceScore", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
    [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetScoreResult), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive", Example = typeof(PriceScoreExample))]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunPriceScore(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        if (!req.TryGetDateParameter(out var date))
            return new BadRequestErrorMessageResult("Date was not on correct format");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var localTime = date.SetHour(req.TryGetHourParameter(out var hour) ? hour : area.CurrentLocalHour());

            var args = new GetScoreArgs(localTime, area, currency);
            var result = await _priceScoreService.GetScore(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult) 
                return actionResult;

            log.LogError(e, "Failed to get price score");
            return new ExceptionResult(e, true);
        }
    }

    [FunctionName("PriceScoreToday")]
    [OpenApiOperation("RunPriceScoreToday", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetScoreResult), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive", Example = typeof(PriceScoreExample))]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunPriceScoreToday(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var localTime = area.LocalTimeNow();

            if (req.TryGetHourParameter(out var hour))
                localTime = localTime.SetHour(hour);

            var args = new GetScoreArgs(localTime, area, currency);
            var result = await _priceScoreService.GetScore(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get price score today");
            return new ExceptionResult(e, true);
        }
    }

        

    [FunctionName("PriceScoreTomorrow")]
    [OpenApiOperation("RunPriceScoreTomorrow", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetScoreResult), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunPriceScoreTomorrow(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var localTime = area.LocalTimeNow().AddDays(1);

            if (req.TryGetHourParameter(out var hour))
                localTime = localTime.SetHour(hour);

            var args = new GetScoreArgs(localTime, area, currency);
            var result = await _priceScoreService.GetScore(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get price score tomorrow");
            return new ExceptionResult(e, true);
        }
    }

    [FunctionName("HourOfPriceScore")]
    [OpenApiOperation("RunHourOfPriceScore", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to to use, for example 2022-01-16")]
    [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetHourResult), Description = "Hour for the price score")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunHourOfPriceScore(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        if (!req.TryGetDateParameter(out var date))
            return new BadRequestErrorMessageResult("Date was not on correct format");

        if (!req.TryGetScoreParameter(out var score))
            return new BadRequestErrorMessageResult("Please supply score to request, example score=1");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var args = new GetHourArgs(date, score, area, currency);
            var result = await _priceScoreService.GetHour(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get hour of price score");
            return new ExceptionResult(e, true);
        }
    }

    [FunctionName("HourOfPriceScoreToday")]
    [OpenApiOperation("RunHourOfPriceScoreToday", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example no2")]
    [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetHourResult), Description = "Hour for the price score")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunHourOfPriceScoreToday(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        if (!req.TryGetScoreParameter(out var score))
            return new BadRequestErrorMessageResult("Please supply score to request, example score=1");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var localTime = area.LocalTimeNow();

            var args = new GetHourArgs(localTime, score, area, currency);
            var result = await _priceScoreService.GetHour(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get hour of price score today");
            return new ExceptionResult(e, true);
        }
    }

    [FunctionName("HourOfPriceScoreTomorrow")]
    [OpenApiOperation("RunHourOfPriceScoreTomorrow", "name", Description = "Description of the function")]
    [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
    [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
    [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
    [OpenApiParameter("format", In = ParameterLocation.Query, Required = false, Type = typeof(Format), Description = "The format to use json or xml")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(GetHourResult), Description = "Hour for the price score")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, "application/xml", typeof(string))]
    [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(Exception))]
    public async Task<IActionResult> RunHourOfPriceScoreTomorrow(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        req.SetAcceptHeadersIfFormatSetToXml();

        if (!req.TryGetAreaParameter(out var area))
            return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

        if (!req.TryGetScoreParameter(out var score))
            return new BadRequestErrorMessageResult("Please supply score to request, example score=1");

        var currency = req.GetCurrencyParameterOrDefault();

        try
        {
            var localTime = area.LocalTimeNow().AddDays(1);

            var args = new GetHourArgs(localTime, score, area, currency);
            var result = await _priceScoreService.GetHour(args);

            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            if (e.HandleException() is { } actionResult)
                return actionResult;

            log.LogError(e, "Failed to get hour of price score tomorrow");
            return new ExceptionResult(e, true);
        }
    }
}