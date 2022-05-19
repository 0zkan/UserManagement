using Management.API.Services;
using UserManagement.Framework.Extensions;
using UserManagement.Services.Management.API.Entities;
using UserManagement.Services.Management.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ManagementDatabaseSettings>(
    builder.Configuration.GetSection("ManagementDatabase")
);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddMongo()
                .AddMongoRepository<User>("Users")
                .AddMongoRepository<RegisterRequest>("RegisterRequests")
                .AddMassTransitWithRabbitMQ();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ManagerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
