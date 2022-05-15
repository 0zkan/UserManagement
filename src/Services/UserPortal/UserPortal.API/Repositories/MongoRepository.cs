using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Repositories
{
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

        public async Task CreateAsync(T newEntity) =>
            await dbCollection.InsertOneAsync(newEntity);

        public async Task UpdateAsync(string id, T updatedEntity) =>
            await dbCollection.ReplaceOneAsync(x => x.Id == id, updatedEntity);

        public async Task RemoveAsync(string id) =>
            await dbCollection.DeleteOneAsync(x => x.Id == id);
    }

}