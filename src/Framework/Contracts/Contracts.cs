namespace UserManagement.Services.Framework.API.Contracts;
public record UserApprovement(Guid id, bool isApproved);
public record UserActivation(Guid id, bool isEnabled);
public record UserRegister(Guid id, string name);