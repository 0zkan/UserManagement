namespace Management.UnitTests.Application;

public class ManagerServiceUnitTest
{
    private readonly Mock<ILogger<ManagerService>> _logger;
    private readonly Mock<IRepository<User>> _userRepository;
    private readonly Mock<IRepository<RegisterRequest>> _registerRequestRepository;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;

    public ManagerServiceUnitTest()
    {
        _logger = new Mock<ILogger<ManagerService>>();
        _userRepository = new Mock<IRepository<User>>();
        _registerRequestRepository = new Mock<IRepository<RegisterRequest>>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
    }

    [Fact]
    public async Task GetUserList_ReturnOkWithUserList()
    {
        // Arrange
        var data = new List<User>{
            new User{
                Id = Guid.NewGuid(),
                Name = "ozkan",
                IsApproved = true,
                IsEnable = true,
            },
            new User{
                Id = Guid.NewGuid(),
                Name = "ozkan2",
                IsApproved = false,
                IsEnable = false,
            }};

        _userRepository.Setup(repo => repo.GetAsync()).Returns(Task.FromResult(data));

        var service = new ManagerService(_logger.Object, _userRepository.Object, _registerRequestRepository.Object, _publishEndpoint.Object);

        // Act
        //var response = await service.GetUserList(new API.UserListRequest { }, TestServerCallContext.Create());

        // Assert
        //mockGreeter.Verify(v => v.Greet("Joe"));
        //Assert.Equal("Hello Joe", response.Message);
    }
}