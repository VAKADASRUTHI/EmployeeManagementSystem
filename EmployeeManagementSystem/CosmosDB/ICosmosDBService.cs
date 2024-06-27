using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagementSystem.Entities;
using Microsoft.Azure.Cosmos;

namespace EmployeeManagementSystem.CosmosDB
{
    public interface ICosmosDBService
    {
        Task<IEnumerable<T>> GetItemsAsync<T>(QueryDefinition query);
        Task<T> GetItemAsync<T>(string id, string partitionKey);
        Task AddItemAsync<T>(T item);
        Task UpdateItemAsync<T>(string id, T item);
        Task DeleteItemAsync<T>(string id, string partitionKey);
    }
}