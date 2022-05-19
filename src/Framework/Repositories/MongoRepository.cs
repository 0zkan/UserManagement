using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserManagement.Framework.Entities;

namespace UserManagement.Framework.Repositories;
public class MongoRepository<T> : IRepository<T> where T : IEntity
{
    private readonly IMongoCollection<T> dbCollection;
    private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        dbCollection = database.GetCollection<T>(collectionName);
    }
    public async Task<List<T>> GetAsync() =>
        await dbCollection.Find(_ => true).ToListAsync();

    public async Task<T?> GetAsync(string name) =>
        await dbCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task<T?> GetAsync(Guid id) =>
        await dbCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(T newEntity) =>
        await dbCollection.InsertOneAsync(newEntity);

    public async Task UpdateAsync(Guid id, T updatedEntity) =>
        await dbCollection.ReplaceOneAsync(x => x.Id == id, updatedEntity);

    public async Task RemoveAsync(Guid id) =>
        await dbCollection.DeleteOneAsync(x => x.Id == id);
};