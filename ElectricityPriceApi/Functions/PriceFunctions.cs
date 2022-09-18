using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using ElectricityPriceApi.Enums;
using ElectricityPriceApi.Services;
using ElectricityPriceApi.Services.Prices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ElectricityPriceApi.Functions
{
    public class PriceFunctions
    {
        private readonly IPriceService _priceService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public PriceFunctions(IPriceService priceService, JsonSerializerSettings jsonSerializerSettings)
        {
            _priceService = priceService;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        [FunctionName("Price")]
        [OpenApiOperation("RunPrice", "name", Description = "Description of the function")]
        [OpenApiParameter("area", In = ParameterLocation.Query, Required = true, Type = typeof(Area), Description = "Price area code, for example NO2")]
        [OpenApiParameter("fromDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
        [OpenApiParameter("toDate", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The date and time to get price score for example 2022-01-16")]
        [OpenApiParameter("currency", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The currency to use for price")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "text/plain", typeof(int), Description = "Price score for the hour. 1 is cheapest and 24 is most expensive")]
        [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RunPrice(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (!Enum.TryParse(req.Query["area"], true, out Area area))
                return new BadRequestErrorMessageResult("Please supply area to request, example area=no2");

            if (!DateTime.TryParse(req.Query["fromDate"], CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            if (!DateTime.TryParse(req.Query["toDate"], CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                return new BadRequestErrorMessageResult("Date was not on correct format");

            var currency = req.Query["currency"];
            if (string.IsNullOrEmpty(currency))
                currency = "EUR";

            try
            {
                var args = new GetHourPricesArgs(area, fromDate, toDate, currency);

                var result = await _priceService.GetHourPrices(args);
                return new JsonResult(result, _jsonSerializerSettings);
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed to get price score");
                return new ExceptionResult(e, true);
            }
        }
    }

}
