namespace UnitTest.UserPortal.Application;

public class AuthControllerTest
{
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IRepository<User>> _userRepository = new();
    private readonly Mock<IPublishEndpoint> _publishEndpoint = new();
    public AuthControllerTest()
    {
        _configuration = new Mock<IConfiguration>();
        _userRepository = new Mock<IRepository<User>>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
    }

    [Fact]
    public async Task Register_WithUnExistingUser_ReturnOk()
    {
        //Arrange   
        var newUser = new UserDto("Ozkan", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my secret key 123"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Register(newUser);

        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("User Registered.", result.Value);
    }

    [Fact]
    public async Task Register_WithExistingUser_ReturnBadRequest()
    {
        //Arrange 
        var data = new User
        {
            Id = Guid.NewGuid(),
            Name = "ozkan"
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(data);

        var newUser = new UserDto("Ozkan", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my secret key 123"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Register(newUser);

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("User exist.", result.Value);
    }

    [Fact]
    public async Task Login_EnabledAndApprovedUserByRightPassword_ReturnOkWithToken()
    {
        //Arrange 
        var data = new User
        {
            Id = Guid.NewGuid(),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = true,
            IsEnable = true,
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(data);

        var userdata = new UserDto("Ozkan", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my top secret key"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Login(userdata);

        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<string>(result.Value);
    }

    [Fact]
    public async Task Login_EnabledAndApprovedUserByWrongPassword_ReturnBadRequest()
    {
        //Arrange 
        var data = new User
        {
            Id = Guid.NewGuid(),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = true,
            IsEnable = true,
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(data);

        var userdata = new UserDto("Ozkan", "12345789");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my top secret key"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Login(userdata);

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("Wrong password.", result.Value);
    }

    [Fact]
    public async Task Login_DisabledUser_ReturnBadRequest()
    {
        //Arrange 
        var data = new User
        {
            Id = Guid.NewGuid(),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = true,
            IsEnable = false,
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(data);

        var userdata = new UserDto("Ozkan", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my top secret key"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Login(userdata);

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("User disabled.", result.Value);
    }

    [Fact]
    public async Task Login_UnApprovedUser_ReturnBadRequest()
    {
        //Arrange 
        var data = new User
        {
            Id = Guid.NewGuid(),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = false,
            IsEnable = false,
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(data);

        var userdata = new UserDto("Ozkan", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my top secret key"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Login(userdata);

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("Waiting approval.", result.Value);
    }

    [Fact]
    public async Task Login_UnExistingUser_ReturnBadRequest()
    {
        //Arrange 
        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var userdata = new UserDto("test", "12345");

        var appSettingsStub = new Dictionary<string, string> {
            {"AppSettings:Token", "my top secret key"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

        var securityService = new SecurityService(configuration);

        var controller = new AuthController(
        _configuration.Object,
        _userRepository.Object,
        securityService,
        _publishEndpoint.Object);

        //Act     
        var actionResult = await controller.Login(userdata);

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
        Assert.Equal("User not found.", result.Value);
    }
}