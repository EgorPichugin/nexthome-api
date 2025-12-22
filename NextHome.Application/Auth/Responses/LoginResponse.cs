namespace NextHome.Application.Auth.Responses;

/// <summary>
/// Represents the response containing user information.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="AccessToken">User access token.</param>
public record LoginResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string AccessToken
);