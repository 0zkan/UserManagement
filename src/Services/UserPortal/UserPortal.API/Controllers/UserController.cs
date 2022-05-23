using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Framework.Repositories;
using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly IHttpContextAccessor _context;
    public UserController(IHttpContextAccessor context, IRepository<User> userRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _userRepository = userRepository;
    }

    [HttpGet("profile"), Authorize(Roles = "User")]
    public async Task<ActionResult<string>> GetProfile()
    {
        var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("No Permission.");
        }

        var user = await _userRepository.GetAsync(new Guid(userId));

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!user.IsApproved || !user.IsEnable)
        {
            return BadRequest("No Permission.");
        }

        return Ok(user.Description);
    }

    [HttpPut("profile"), Authorize(Roles = "User")]
    public async Task<ActionResult<string>> UpdateProfile([FromBody] UserProfileDto request)
    {
        var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest();
        }

        Guid id = (Guid.TryParse(userId, out Guid guid) && guid != Guid.Empty) ?
            guid : new Guid(userId);

        var user = await _userRepository.GetAsync(id);

        if (user == null || !user.IsApproved || !user.IsEnable)
        {
            return BadRequest("No Permission.");
        }

        user.Description = request.Description;
        await _userRepository.UpdateAsync(id, user);

        return Ok("Updated");
    }
}