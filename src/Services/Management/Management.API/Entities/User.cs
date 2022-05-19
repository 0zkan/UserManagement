using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserManagement.Framework.Entities;

namespace UserManagement.Services.Management.API.Entities;

public class User : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    public bool IsEnable { get; set; } = false;

}
