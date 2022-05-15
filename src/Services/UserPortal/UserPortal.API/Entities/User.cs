using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Entities
{
    public class User : IUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string UserName { get; set; } = null!;

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = null!;

        [BsonElement("PasswordSalt")]
        public string PasswordSalt { get; set; } = null!;

        [BsonElement("IsApproved")]
        public bool IsApproved { get; set; } = false;

        [BsonElement("IsEnable")]
        public bool IsEnable { get; set; } = false;

        [BsonElement("Description")]
        public string Description { get; set; } = null!;

    }
}