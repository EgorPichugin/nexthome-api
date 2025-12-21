namespace NextHome.Application.Auth.Register;

/// <summary>
/// Register response.
/// </summary>
/// <param name="UserId"></param>
/// <param name="Email"></param>
/// <param name="FirstName"></param>
/// <param name="LastName"></param>
public record RegisterResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName
);
