using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsync(string name);
        Task CreateAsync(T newEntity);
        Task UpdateAsync(string id, T updateEntity);
        Task RemoveAsync(string id);
    }

}