using MassTransit;
using UserManagement.Framework.Repositories;
using UserManagement.Services.Framework.API.Contracts;
using UserManagement.Services.Management.API.Entities;

namespace UserManagement.Services.Management.API.Consumers;

public class UserRegisterConsumer : IConsumer<UserRegister>
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<RegisterRequest> _registerRequestRepository;
    public UserRegisterConsumer(IRepository<User> userRepository, IRepository<RegisterRequest> registerRequestRepository)
    {
        _userRepository = userRepository;
        _registerRequestRepository = registerRequestRepository;
    }
    public async Task Consume(ConsumeContext<UserRegister> context)
    {
        var message = context.Message;

        await _registerRequestRepository.CreateAsync(new RegisterRequest
        {
            Id = message.id,
            Name = message.name,
            Status = "Requested",
            CreatedDate = DateTime.Today,
            ModifiedBy = "System"
        });

        await _userRepository.CreateAsync(new User
        {
            Id = message.id,
            Name = message.name,
            IsApproved = false,
            IsEnable = false
        });
    }
}