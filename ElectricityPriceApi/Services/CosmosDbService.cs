using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectricityPriceApi.Configuration;
using ElectricityPriceApi.HttpClients;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ElectricityPriceApi.Services;

public class CosmosDbService : ICosmosDbService
{
    private Container? _container;
    private readonly CosmosConfiguration _cosmosConfiguration;

    public CosmosDbService(IOptions<CosmosConfiguration> cosmosConfiguration)
    {
        _cosmosConfiguration = cosmosConfiguration.Value;
    }

    public async Task Connect()
    {
        var databaseName = _cosmosConfiguration.DatabaseName;
        var containerName = _cosmosConfiguration.ContainerName;
        var account = _cosmosConfiguration.Account;
        var key = _cosmosConfiguration.Key;
        var client = new CosmosClient(account, key);
        var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

        _container = client.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync(HttpClients.Prices prices)
    {
        if (_container is null)
            throw new Exception("Container has not been initialized");

        await _container.CreateItemAsync(prices);
    }

    public async Task DeleteItemAsync(string id)
    {
        if (_container is null)
            throw new Exception("Container has not been initialized");

        await _container.DeleteItemAsync<HttpClients.Prices>(id, new PartitionKey(id));
    }

    public async Task<HttpClients.Prices> GetItemAsync(string id)
    {
        if (_container is null)
            throw new Exception("Container has not been initialized");

        var response = await _container.ReadItemAsync<HttpClients.Prices>(id, new PartitionKey(id));
        return response.Resource;
    }

    public async Task<IEnumerable<HttpClients.Prices>> GetItemsAsync(string queryString)
    {
        if (_container is null)
            throw new Exception("Container has not been initialized");

        var query = _container.GetItemQueryIterator<HttpClients.Prices>(new QueryDefinition(queryString));
        var results = new List<HttpClients.Prices>();
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();

            results.AddRange(response.ToList());
        }

        return results;
    }

    public async Task UpdateItemAsync(string id, HttpClients.Prices prices)
    {
        if (_container is null)
            throw new Exception("Container has not been initialized");

        await _container.UpsertItemAsync(prices, new PartitionKey(id));
    }
}