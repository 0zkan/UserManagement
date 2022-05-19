namespace UserManagement.Services.Management.API.Contracts;

public record UserApprovement(Guid id, bool isApproved);

public record UserActivation(Guid id, bool isEnabled);
