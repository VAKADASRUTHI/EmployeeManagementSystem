using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementSystem.Entities;

namespace EmployeeManagementSystem.CosmosDB
{
    public class CosmosDBService : ICosmosDBService
    {
        private readonly Container _container;

        public CosmosDBService(CosmosClient cosmosClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(QueryDefinition query)
        {
            var queryIterator = _container.GetItemQueryIterator<T>(query);
            var results = new List<T>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<T> GetItemAsync<T>(string id, string partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task AddItemAsync<T>(T item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(typeof(T).GetProperty("Id").GetValue(item).ToString()));
        }

        public async Task UpdateItemAsync<T>(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(id));
        }

        public async Task DeleteItemAsync<T>(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
        }
    }
}
