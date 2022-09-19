using Microsoft.Azure.Cosmos;
using TandemBEProject.DAL.Cosmos;

namespace TandemBEProject
{
    internal static class CosmosHelper
    {
        internal static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(
            IConfigurationSection configurationSection
        )
        {
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;

            CosmosClient client = new(account, key);
            CosmosDbService cosmosDbService = new(client, databaseName, containerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

            return cosmosDbService;
        }
    }
}
