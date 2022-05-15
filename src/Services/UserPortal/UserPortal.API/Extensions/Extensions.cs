using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Extensions
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto(user.UserName, user.PasswordHash);
        }
    }
}