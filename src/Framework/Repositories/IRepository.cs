using UserManagement.Framework.Entities;

namespace UserManagement.Framework.Repositories;
public interface IRepository<T> where T : IEntity
{
    Task<List<T>> GetAsync();
    Task<T?> GetAsync(string name);
    Task CreateAsync(T newEntity);
    Task UpdateAsync(Guid id, T updateEntity);
    Task RemoveAsync(Guid id);
}
