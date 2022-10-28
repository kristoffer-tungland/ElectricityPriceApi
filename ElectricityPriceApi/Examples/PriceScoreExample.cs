using ElectricityPriceApi.Services.Scores;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace ElectricityPriceApi.Examples;

public class PriceScoreExample : OpenApiExample<GetScoreResult>
{
    public override IOpenApiExample<GetScoreResult> Build(NamingStrategy? namingStrategy = null)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Cheapest", new GetScoreResult{ ScoreNow = 1 }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Most expensive", new GetScoreResult { ScoreNow = 24}, namingStrategy));

        return this;
    }
}