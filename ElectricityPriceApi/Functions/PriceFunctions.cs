using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Examples;
using ElectricityPriceApi.Extensions;
using ElectricityPriceApi.Services;
using ElectricityPriceApi.Services.Prices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ElectricityPriceApi.Functions
{
    public class PriceFunctions
    {
        private readonly IPriceService _priceService;

        public PriceFunctions(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [FunctionName("Price")]
        [OpenApiOperation("RunPrice", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("fromDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
        [OpenApiParameter("toDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunPrice(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!Enum.TryParse(req.Query["area"], true, out Area area))
                return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

            if (!DateTimeOffset.TryParse(req.Query["fromDate"], CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            if (!DateTimeOffset.TryParse(req.Query["toDate"], CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            try
            {
                var args = new GetHourPricesArgs(area, fromDate, toDate);

                var hourPrices = await _priceService.GetHourPrices(args);
                return new OkObjectResult(hourPrices);
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed to get price score");
                return new ExceptionResult(e, true);
            }
        }
    }

}
