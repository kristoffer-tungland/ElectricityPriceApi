using System.Collections.Generic;
using System.Threading.Tasks;
using ElectricityPriceApi.HttpClients;

namespace ElectricityPriceApi.Services;

public class PriceRepository
{
    private readonly ICosmosDbService _cosmosDbService;

    public PriceRepository(ICosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    public async Task AddItemAsync(HttpClients.Prices item)
    {
        await _cosmosDbService.AddItemAsync(item);
    }

    public async Task DeleteItemAsync(string id)
    {
        await _cosmosDbService.DeleteItemAsync(id);
    }

    public async Task<HttpClients.Prices> GetItemAsync(string id)
    {
        return await _cosmosDbService.GetItemAsync(id);

    }

    public async Task<IEnumerable<HttpClients.Prices>> GetItemsAsync(string queryString)
    {
        return await _cosmosDbService.GetItemsAsync(queryString);
    }

    public async Task UpdateItemAsync(string id, HttpClients.Prices item)
    {
        await _cosmosDbService.UpdateItemAsync(id, item);
    }
}