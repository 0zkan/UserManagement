global using UserManagement.Services.UserPortal.API.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Filters;
using UserManagement.Services.UserPortal.API.Entities;
using UserManagement.Services.UserPortal.API.Extensions;
using UserManagement.Services.UserPortal.API.Models;
using UserManagement.Services.UserPortal.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UserPortalDatabaseSettings>(
    builder.Configuration.GetSection("UserPortalDatabase"));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standart Authorization header using the Bearer scheme (\"bearer {token}\")",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }

);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddMongo().AddMongoRepository<User>("Users");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
