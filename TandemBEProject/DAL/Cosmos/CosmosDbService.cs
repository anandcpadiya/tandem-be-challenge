using TandemBEProject.Models;

namespace TandemBEProject.DAL.Cosmos
{
    public class CosmosDbService : IDbService
    {
        public Task AddUser(UserModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel?> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
