using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace ElectricityPriceApi.Functions
{
    public class CosmosDbWriteFunction
    {
        private readonly ILogger<CosmosDbWriteFunction> _logger;

        public CosmosDbWriteFunction(ILogger<CosmosDbWriteFunction> log)
        {
            _logger = log;
        }

        [FunctionName("CosmosDbWriteFunction")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "ToDoList",
                collectionName: "Items",
                ConnectionStringSetting = "CosmosDBConnection")]out dynamic document)
        {

            document = new { Description = "queueMessage", id = Guid.NewGuid() };

            _logger.LogInformation($"C# Queue trigger function inserted one row");

            return new OkResult();
        }
    }
}

