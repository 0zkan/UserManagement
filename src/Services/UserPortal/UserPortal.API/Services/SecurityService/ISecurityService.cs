using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Services;
public interface ISecurityService
{
    public string CreateToken(User user);
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public bool VerifyPasswordHash(string password, string hash, string salt);
}