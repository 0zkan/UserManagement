using System.Security.Claims;

namespace UnitTest.UserPortal.Application;

public class UserControllerTest
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<IRepository<User>> _userRepository;

    public UserControllerTest()
    {
        _userRepository = new Mock<IRepository<User>>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Fact]
    public async Task GetProfile_WithVerifiedUser_ReturnOk()
    {
        //Arrange 
        _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
            .Returns(new Claim(ClaimTypes.NameIdentifier, "1ae123ff-cdca-41a5-8949-17043ff4a13d"));

        var data = new User
        {
            Id = new Guid("1ae123ff-cdca-41a5-8949-17043ff4a13d"),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = true,
            IsEnable = true,
            Description = "My Description"
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(data);

        var controller = new UserController(_httpContextAccessor.Object, _userRepository.Object);

        //Act     
        var actionResult = await controller.GetProfile();

        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<string>(result.Value);
    }

    [Fact]
    public async Task GetProfile_WithUnVerifiedUser_ReturnBadRequest()
    {
        //Arrange 
        _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
            .Returns(new Claim(ClaimTypes.NameIdentifier, ""));

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User)null);

        var controller = new UserController(_httpContextAccessor.Object, _userRepository.Object);

        //Act     
        var actionResult = await controller.GetProfile();

        //Assert
        var result = actionResult.Result as BadRequestObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        Assert.IsType<string>(result.Value);
    }

    [Fact]
    public async Task UpdateProfile_WithDescription_ReturnOk()
    {
        //Arrange 
        _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
            .Returns(new Claim(ClaimTypes.NameIdentifier, "1ae123ff-cdca-41a5-8949-17043ff4a13d"));

        var data = new User
        {
            Id = new Guid("1ae123ff-cdca-41a5-8949-17043ff4a13d"),
            Name = "ozkan",
            PasswordHash = "Q7kiKtYEUqxX60PGboZsLOVhZpcehvJY1z2z7MIa40mKKDs+FrR9E1PGW2/mQ7GZpLTOg8vvQ3QT2QYczU2DkA==",
            PasswordSalt = "97mRDl0Csuo9l6NCOoDaPVJgci6L7Wtjbv29uHZdmasvl/TpPF29+ay8RF02pT3JJKllcIooc8fMPtBvpwnDgRxRJ9YTSEuGDhgyQdRJQb9GIrq3idT8iJXusHMoebBl2XJ0ejSrup/v+vWOitC7hQHTDxmMLN/f3M8Kn/fJ9q0=",
            IsApproved = true,
            IsEnable = true,
            Description = "My Description"
        };

        _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(data);

        UserProfileDto updateData = new UserProfileDto
        {
            Description = "Changed Description"
        };

        var controller = new UserController(_httpContextAccessor.Object, _userRepository.Object);

        //Act     
        var actionResult = await controller.UpdateProfile(updateData);

        //Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.IsType<string>(result.Value);
    }
}