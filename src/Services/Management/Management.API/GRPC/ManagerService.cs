using Grpc.Core;
using MassTransit;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Framework.API.Contracts;
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

        var userList = await _userRepository.GetAsync();
        foreach (var item in userList)
        {
            result.Data.Add(new UserModel { Id = item.Id.ToString(), Name = item.Name, Isenabled = item.IsEnable, Isapprovement = item.IsApproved });
        }

        return result;
    }

    public override async Task<UserApproveResponse> UserApprovement(UserApproveRequest request, ServerCallContext context)
    {
        UserApproveResponse result = new UserApproveResponse();
        //Todo : check user id is null or empty
        var user = await _userRepository.GetAsync(new Guid(request.Id));
        if (user != null && request.Isapproved)
        {
            user.IsApproved = true;
            user.IsEnable = true;
            await _userRepository.UpdateAsync(user.Id, user);
            await _publishEndpoint.Publish(new UserApprovement(user.Id, true));
        }

        result.Issuccess = true;
        result.Message = "User confirmed.";

        return result;
    }

    public override async Task<UserActivationResponse> UserActivation(UserActivationRequest request, ServerCallContext context)
    {
        UserActivationResponse result = new UserActivationResponse();

        //Todo : check user id is null or empty
        var user = await _userRepository.GetAsync(new Guid(request.Id));
        if (user != null)
        {
            user.IsEnable = request.Isneabled;
            await _userRepository.UpdateAsync(user.Id, user);
            await _publishEndpoint.Publish(new UserActivation(user.Id, request.Isneabled));
        }

        result.Issuccess = true;
        result.Message = request.Isneabled ? "User enabled." : "User disabled";

        return result;
    }
}
