using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserManagement.Framework.Entities;

namespace UserManagement.Services.Management.API.Entities;
public class RegisterRequest : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Requested, Accepted, Declined
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
