using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ElectricityPriceApi
{
    public static class CurrentPriceScore
    {
        [FunctionName("CurrentPriceScore")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var hour = int.TryParse(req.Query["hour"], out var result) ? result : DateTime.Now.Hour;

            try
            {
                var score = PriceObject.GetScore(hour);
                return new OkObjectResult(score);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }
    }

    public static class PriceObject
    {
        public static Dictionary<int, float> Prices { get; } = CreateDesignData();

        private static Dictionary<int, float> CreateDesignData()
        {
            var random = new Random();
            var result = new Dictionary<int, float>();

            for (var i = 1; i <= 24; i++)
            {
                var randomNumber = random.Next(0, 24);

                result.Add(i, randomNumber);
            }

            return result;
        }

        public static int GetScore(int hour)
        {
            if (Prices.TryGetValue(hour, out var value))
            {
                var orderedList = Prices.Select(x => x.Value).OrderByDescending(x => x).ToList();
                
                for (var i = 0; i < orderedList.Count(); i++)
                {
                    if (orderedList[i].Equals(value))
                        return i;
                }
            }

            throw new Exception($"Could not get score from hour {hour}");

        }
    }
}
