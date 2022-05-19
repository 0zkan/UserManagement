using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPut("{id}/profile"), Authorize(Roles = "User")]
    public async Task<ActionResult<string>> Profile(UserProfileDto request)
    {
        //Kullanıcının enable olması gerekiyor, bunu her durumda yapmak lazım.
        //custom attribute ile kontrol edilebilir

        return Ok("Updated");
    }
}