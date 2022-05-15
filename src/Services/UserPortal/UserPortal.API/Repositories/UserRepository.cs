using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string collectionName = "Users";
        private readonly IMongoCollection<User> dbCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;

        public UserRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("UserPortal");
            dbCollection = mongoDatabase.GetCollection<User>(collectionName);
        }
        public async Task<List<User>> GetAsync() =>
            await dbCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string userName) =>
            await dbCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await dbCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await dbCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await dbCollection.DeleteOneAsync(x => x.Id == id);
    }

}