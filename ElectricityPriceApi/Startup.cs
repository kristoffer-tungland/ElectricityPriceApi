using System;
using System.Reflection;
using ElectricityPriceApi;
using ElectricityPriceApi.Configuration;
using ElectricityPriceApi.HttpClients;
using ElectricityPriceApi.Services;
using ElectricityPriceApi.Services.Prices;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ElectricityPriceApi;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddMvcCore().AddNewtonsoftJson(x =>
        {
            x.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

        builder.Services.AddHttpClient();
        builder.Services.AddTransient<PriceHttpClient>();
        builder.Services.AddTransient<EntsoeHttpClient>();


        builder.Services.AddScoped<PriceScoreService>();
        builder.Services.AddScoped<IPriceService, PriceService>();

        builder.Services.AddOptions<MyConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("MyConfiguration").Bind(settings);
            });

        builder.Services.AddOptions<MyConfigurationSecrets>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("MyConfigurationSecrets").Bind(settings);
            });

        
        builder.Services.AddOptions<CosmosConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("Cosmos").Bind(settings);
            });

        builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
        

        builder.Services.AddSingleton<PriceRepository>();


    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        var builtConfig = builder.ConfigurationBuilder.Build();
        var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

        if (!string.IsNullOrEmpty(keyVaultEndpoint))
        {
            // https://damienbod.com/2020/07/20/using-key-vault-and-managed-identities-with-azure-functions/
            builder.ConfigurationBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddAzureKeyVault(keyVaultEndpoint)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
        else
        {
            // local dev no Key Vault
            builder.ConfigurationBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}


public class MyConfigurationSecrets
{
    public string? ServiceApiKey { get; set; }
    public string? PriceApiKey { get; set; }
    public string? EntsoeApiKey { get; set; }
}

public class MyConfiguration
{
    public string Name { get; set; }
    public int AmountOfRetries { get; set; }
}