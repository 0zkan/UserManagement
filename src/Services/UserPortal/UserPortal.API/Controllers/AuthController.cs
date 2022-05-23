using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Framework.API.Contracts;
using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Models;

namespace UserManagement.Services.UserPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<User> _userRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public ISecurityService _securityService { get; }

    public AuthController(IConfiguration configuration, IRepository<User> userRepository, ISecurityService securityService, IPublishEndpoint publishEndpoint)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _securityService = securityService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<string>> Register(UserDto request)
    {
        var existedUser = await _userRepository.GetAsync(request.UserName.ToLower());

        if (existedUser != null)
        {
            return BadRequest("User exist.");
        }

        _securityService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new User();
        user.Id = Guid.NewGuid();
        user.Name = request.UserName.ToLower();
        user.PasswordHash = Convert.ToBase64String(passwordHash, 0, passwordHash.Length);
        user.PasswordSalt = Convert.ToBase64String(passwordSalt, 0, passwordSalt.Length);
        user.IsApproved = false;
        user.IsEnable = false;
        user.Description = string.Empty;

        await _userRepository.CreateAsync(user);

        await _publishEndpoint.Publish(new UserRegister(user.Id, user.Name));

        return Ok("User Registered.");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var user = await _userRepository.GetAsync(request.UserName.ToLower());
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!_securityService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        if (!user.IsApproved)
        {
            return BadRequest("Waiting approval.");
        }

        if (!user.IsEnable)
        {
            return BadRequest("User disabled.");
        }

        string token = _securityService.CreateToken(user);

        return Ok(token);
    }

}