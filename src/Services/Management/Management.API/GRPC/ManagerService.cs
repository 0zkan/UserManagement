using Grpc.Core;
using MassTransit;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Management.API.Contracts;
using UserManagement.Services.Management.API.Entities;

namespace Management.API.Services;

public class ManagerService : Manager.ManagerBase
{
    private readonly ILogger<ManagerService> _logger;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<RegisterRequest> _registerRequestRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public ManagerService(ILogger<ManagerService> logger, IRepository<User> userRepository, IRepository<RegisterRequest> registerRequestRepository, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _userRepository = userRepository;
        _registerRequestRepository = registerRequestRepository;
        _publishEndpoint = publishEndpoint;
    }

    public override async Task<UserListResponse> GetUserList(UserListRequest request, ServerCallContext context)
    {
        UserListResponse result = new UserListResponse();
        result.Data.Add(new UserModel { Id = 1, Name = "Ozkan", Isenabled = false, Isapprovement = false });
        result.Data.Add(new UserModel { Id = 2, Name = "Hakan", Isenabled = false, Isapprovement = false });

        await _publishEndpoint.Publish(new UserApprovement(Guid.NewGuid(), true));

        return result;
    }

    public override async Task<UserApproveResponse> UserApprovement(UserApproveRequest request, ServerCallContext context)
    {
        UserApproveResponse result = new UserApproveResponse();

        return result;
    }

    public override async Task<UserActivationResponse> UserActivation(UserActivationRequest request, ServerCallContext context)
    {
        UserActivationResponse result = new UserActivationResponse();

        return result;
    }
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        HelloReply result = new HelloReply();
        result.Message = "Helloo";

        await _publishEndpoint.Publish(new UserApprovement(Guid.NewGuid(), true));

        return result;
    }
}
