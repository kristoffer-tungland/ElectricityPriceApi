using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectricityPriceApi.Services;

public interface ICosmosDbService
{
    Task Connect();
    Task<IEnumerable<HttpClients.Prices>> GetItemsAsync(string query);
    Task<HttpClients.Prices> GetItemAsync(string id);
    Task AddItemAsync(HttpClients.Prices item);
    Task UpdateItemAsync(string id, HttpClients.Prices prices);
    Task DeleteItemAsync(string id);
}