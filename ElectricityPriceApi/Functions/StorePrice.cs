using System.Net;
using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace ElectricityPriceApi.Functions
{
    public class StorePrice
    {
        private readonly ILogger<StorePrice> _logger;
        private readonly PriceHttpClient _priceHttpClient;

        public StorePrice(ILogger<StorePrice> log, PriceHttpClient priceHttpClient)
        {
            _logger = log;
            _priceHttpClient = priceHttpClient;
        }

        [FunctionName("StorePrice")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "ToDoList",
                collectionName: "Items",
                ConnectionStringSetting = "CosmosDBConnection")]IAsyncCollector<Prices> pricesOut)
        {
            var prices = await _priceHttpClient.GetDesignData();

            if (prices is null)
                return new NotFoundResult();

            foreach (var price in prices)
            {
                _logger.LogInformation($"Price added");
                await pricesOut.AddAsync(price);
            }

            return new OkResult();
        }
    }
}

