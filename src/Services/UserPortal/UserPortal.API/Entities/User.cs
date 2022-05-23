using UserManagement.Framework.Entities;

namespace UserManagement.Services.UserPortal.API.Entities;
public class User : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public bool IsApproved { get; set; } = false;
    public bool IsEnable { get; set; } = false;
    public string Description { get; set; } = null!;
}
