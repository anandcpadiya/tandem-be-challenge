using Microsoft.Azure.Cosmos;
using TandemBEProject.DAL.Exceptions;
using TandemBEProject.Models;

namespace TandemBEProject.DAL.Cosmos
{
    public class CosmosDbService : IDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddUser(UserModel model)
        {
            try
            {
                await _container.CreateItemAsync(model, new PartitionKey(model.EmailAddress));
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new UserExistsException(string.Format("A user with {0} email already exists.", model.EmailAddress));
            }
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            try
            {
                ItemResponse<UserModel> response = await _container.ReadItemAsync<UserModel>(email, new PartitionKey(email));

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                await _container.ReadContainerAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UserModel> UpdateUserByEmail(UserModel model)
        {
            try
            {
                ItemResponse<UserModel> response = await _container.ReadItemAsync<UserModel>(
                    model.EmailAddress, new PartitionKey(model.EmailAddress)
                );

                UserModel existingModel = response.Resource;
                existingModel.FirstName = model.FirstName;
                existingModel.MiddleName = model.MiddleName;
                existingModel.LastName = model.LastName;
                existingModel.Name = model.Name;
                existingModel.PhoneNumber = model.PhoneNumber;

                return await _container.UpsertItemAsync(existingModel);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new UserNotFoundException(string.Format("Did not find any user with {0} email.", model.EmailAddress));
            }
        }
    }
}
