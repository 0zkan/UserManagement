using MassTransit;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Framework.API.Contracts;
using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Consumers;

public class UserApprovementConsumer : IConsumer<UserApprovement>
{
    private readonly IRepository<User> _userRepository;
    public UserApprovementConsumer(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    public Task Consume(ConsumeContext<UserApprovement> context)
    {
        throw new NotImplementedException();
    }
}