namespace UserManagement.Services.UserPortal.API.Models
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserDto()
        {

        }
        public UserDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}