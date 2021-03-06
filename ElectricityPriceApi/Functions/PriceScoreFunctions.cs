using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
                var date = DateTime.Parse(req.Query["date"]);

                if (int.TryParse(req.Query["hour"], out var hour))
                    date = date.SetHour(hour);

                var score = await _priceScoreService.GetScore(date);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("PriceScoreToday")]
        [OpenApiOperation("RunPriceScoreToday", "name", Description = "Description of the function")]
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
                var date = DateExtensions.Today;

                if (int.TryParse(req.Query["hour"], out var hour))
                    date = date.SetHour(hour);

                var score = await _priceScoreService.GetScore(date);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }


        [FunctionName("PriceScoreTomorrow")]
        [OpenApiOperation("RunPriceScoreTomorrow", "name", Description = "Description of the function")]
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
                var date = DateExtensions.Tomorrow;

                if (int.TryParse(req.Query["hour"], out var hour))
                    date = date.SetHour(hour);

                var score = _priceScoreService.GetScore(date);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScore")]
        [OpenApiOperation("RunHourOfPriceScore", "name", Description = "Description of the function")]
        [OpenApiParameter("date", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to to use, for example 2022-01-16")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScore(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!DateTime.TryParse(req.Query["date"], out var date))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(date, score);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScoreToday")]
        [OpenApiOperation("RunHourOfPriceScoreToday", "name", Description = "Description of the function")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScoreToday(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(DateTime.Today, score);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScoreTomorrow")]
        [OpenApiOperation("RunHourOfPriceScoreTomorrow", "name", Description = "Description of the function")]
        [OpenApiParameter("score", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The price score to get the hour for, between 1-24. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Hour for the price score", Example = typeof(PriceScoreExample))]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunHourOfPriceScoreTomorrow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!int.TryParse(req.Query["score"], out var score))
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");

            try
            {
                var hour = await _priceScoreService.GetHour(DateTime.Today.AddDays(1), score);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }
    }
}
