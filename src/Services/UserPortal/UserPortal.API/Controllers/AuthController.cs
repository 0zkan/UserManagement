using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public IUserService _userService { get; }

    public AuthController(IConfiguration configuration, IRepository<User> userRepository, IUserService userService, IPublishEndpoint publishEndpoint)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _userService = userService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        try
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Id = Guid.NewGuid();
            user.Name = request.UserName;
            user.PasswordHash = Convert.ToBase64String(passwordHash, 0, passwordHash.Length);
            user.PasswordSalt = Convert.ToBase64String(passwordSalt, 0, passwordSalt.Length);
            await _userRepository.CreateAsync(user);

            await _publishEndpoint.Publish(new UserRegister(user.Id, user.Name));

            return Ok("User Registered");
        }
        catch (System.Exception)
        {
            return Ok("An error occurred while processing your transaction.");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var user = await _userRepository.GetAsync(request.UserName);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        string token = CreateToken(user);

        return Ok(token);
    }
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "User")
            };
        //TODO : token should be in key secret store
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value
        ));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, string hash, string salt)
    {
        byte[] passwordHash = Convert.FromBase64String(hash);
        byte[] passwordSalt = Convert.FromBase64String(salt);

        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}