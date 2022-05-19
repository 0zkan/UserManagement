using MongoDB.Driver;
using Microsoft.Extensions.Options;
using UserManagement.Services.UserPortal.API.Models;
using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Services;
public class UserService : IUserService
{

    public UserService(
        IOptions<UserPortalDatabaseSettings> userPortalDatabaseSettings)
    {

    }
}
