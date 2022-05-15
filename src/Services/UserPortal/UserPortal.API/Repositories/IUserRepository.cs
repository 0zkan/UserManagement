using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAsync();
        Task<User?> GetAsync(string id);
        Task CreateAsync(User newUser);
        Task UpdateAsync(string id, User updatedUser);
        Task RemoveAsync(string id);
    }

}