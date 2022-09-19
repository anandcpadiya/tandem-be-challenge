using TandemBEProject.Models;

namespace TandemBEProject.DAL
{
    public interface IDbService
    {
        Task AddUser(UserModel model);

        Task<UserModel?> GetUserByEmail(string email);

        Task<UserModel> UpdateUserByEmail(UserModel model);
    }
}
