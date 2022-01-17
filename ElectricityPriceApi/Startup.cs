using System;
using System.Reflection;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ElectricityPriceApi;
using ElectricityPriceApi.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ElectricityPriceApi;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<PriceScoreService>();

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
    public string ServiceApiKey { get; set; }
}

public class MyConfiguration
{
    public string Name { get; set; }
    public int AmountOfRetries { get; set; }
}