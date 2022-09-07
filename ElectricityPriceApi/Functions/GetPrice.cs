using System.IO;
using System.Net;
using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ElectricityPriceApi.Functions
{
    public class GetPrice
    {
        private readonly ILogger<GetPrice> _logger;
        private readonly PriceHttpClient _priceHttpClient;

        public GetPrice(ILogger<GetPrice> log, PriceHttpClient priceHttpClient)
        {
            _logger = log;
            _priceHttpClient = priceHttpClient;
        }

        [FunctionName("GetPrice")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var prices = await _priceHttpClient.GetDesignData();
            
            return new OkObjectResult(JsonConvert.SerializeObject(prices, Formatting.Indented));
        }
    }
}

