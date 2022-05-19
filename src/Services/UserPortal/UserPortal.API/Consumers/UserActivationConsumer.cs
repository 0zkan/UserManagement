using MassTransit;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Framework.API.Contracts;
using UserManagement.Services.UserPortal.API.Entities;

namespace UserManagement.Services.UserPortal.API.Consumers;

public class UserActivationConsumer : IConsumer<UserActivation>
{
    private readonly IRepository<User> _userRepository;
    public UserActivationConsumer(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task Consume(ConsumeContext<UserActivation> context)
    {
        var message = context.Message;

        //await _userRepository.GetAsync());
    }
}