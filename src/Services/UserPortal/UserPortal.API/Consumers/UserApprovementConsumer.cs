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
    public async Task Consume(ConsumeContext<UserApprovement> context)
    {
        var message = context.Message;

        var user = await _userRepository.GetAsync(message.id);
        if (user != null && message.isApproved)
        {
            user.IsEnable = true;
            user.IsApproved = true;
            await _userRepository.UpdateAsync(user.Id, user);
        }
    }
}