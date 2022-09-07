using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Examples;
using ElectricityPriceApi.Extensions;
using ElectricityPriceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ElectricityPriceApi.Functions
{
    public class PriceScoreFunctions
    {
        private readonly PriceScoreService _priceScoreService;

        public PriceScoreFunctions(PriceScoreService priceScoreService)
        {
            _priceScoreService = priceScoreService;
        }

        [FunctionName("PriceScore")]
        [OpenApiOperation("RunPriceScore", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
        [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunPriceScore(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;
                var date = DateTime.Parse(req.Query["date"], CultureInfo.InvariantCulture);
                var dateTime = date.SetHour(int.TryParse(req.Query["hour"], out var hour) ? hour : area.GetCurrentLocalHour());

                var localTime = dateTime.ToLocalTime(area);

                var score = await _priceScoreService.GetScore(localTime, area);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("PriceScoreToday")]
        [OpenApiOperation("RunPriceScoreToday", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunPriceScoreToday(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;
                var date = DateExtensions.Today;
                var dateTime = date.SetHour(int.TryParse(req.Query["hour"], out var hour) ? hour : area.GetCurrentLocalHour());

                var localTime = dateTime.ToLocalTime(area);

                var score = await _priceScoreService.GetScore(localTime, area);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }


        [FunctionName("PriceScoreTomorrow")]
        [OpenApiOperation("RunPriceScoreTomorrow", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("hour", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The hour to get price score for, between 0-23, if blank use current hour")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunPriceScoreTomorrow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;
                var date = DateExtensions.Tomorrow;
                var dateTime = date.SetHour(int.TryParse(req.Query["hour"], out var hour) ? hour : area.GetCurrentLocalHour());

                var localTime = dateTime.ToLocalTime(area);

                var score = await _priceScoreService.GetScore(localTime, area);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScore")]
        [OpenApiOperation("RunHourOfPriceScore", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to to use, for example 2022-01-16")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScore(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;

            if (!DateTime.TryParse(req.Query["date"], out var date))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(date, score, area);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScoreToday")]
        [OpenApiOperation("RunHourOfPriceScoreToday", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScoreToday(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(DateTime.Today, score, area);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScoreTomorrow")]
        [OpenApiOperation("RunHourOfPriceScoreTomorrow", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScoreTomorrow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var area = Enum.TryParse(req.Query["area"], out Area res) ? res : Area.No2;

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(DateExtensions.Tomorrow, score, area);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }
    }
}
