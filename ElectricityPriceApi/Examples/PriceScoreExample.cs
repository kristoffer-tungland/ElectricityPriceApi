using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace ElectricityPriceApi.Examples;

public class PriceScoreExample : OpenApiExample<int>
{
    public override IOpenApiExample<int> Build(NamingStrategy namingStrategy = null)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Cheapest", 1, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Most expensive", 24, namingStrategy));

        return this;
    }
}