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
    public static class PriceScoreFunctions
    {
        [FunctionName("PriceScoreToday")]
        public static async Task<IActionResult> RunPriceScoreToday(
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

        [FunctionName("PriceScoreTomorrow")]
        public static async Task<IActionResult> RunPriceScoreTomorrow(
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

        [FunctionName("HourOfPriceScoreToday")]
        public static async Task<IActionResult> RunHourOfPriceScoreToday(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int? score = int.TryParse(req.Query["score"], out var result) ? result : null;

            if (score is null)
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");
            try
            {
                var hour = PriceObject.GetHour((int)score);
                return new OkObjectResult(hour);
            }
            catch (Exception e)
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("HourOfPriceScoreTomorrow")]
        public static async Task<IActionResult> RunHourOfPriceScoreTomorrow(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int? score = int.TryParse(req.Query["score"], out var result) ? result : null;

            if (score is null)
                return new BadRequestErrorMessageResult("Please supply score to request, example ?score=1");
            try
            {
                var hour = PriceObject.GetHour((int)score);
                return new OkObjectResult(hour);
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
                var orderedList = Prices.OrderBy(x => x.Value).ToList();

                var pair = orderedList.First(x => x.Key == hour);

                var index = orderedList.IndexOf(pair);

                return index + 1;
            }

            throw new Exception($"Could not get score from hour {hour}");

        }

        public static int GetHour(int score)
        {
            var key = score - 1;

            var orderedList = Prices.OrderBy(x => x.Value).ToList();

            if (orderedList.Count >= key)
            {
                var pair = orderedList[key];
                
                return pair.Key;
            }

            throw new Exception($"Could not get hour from score {score}");
        }
    }
}
