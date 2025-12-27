namespace NextHome.Application.Auth.Responses;

/// <summary>
/// Represents the response containing user information.
/// </summary>
/// <param name="User">Authorised user.</param>
/// <param name="AccessToken">User access token.</param>
public record LoginResponse(
    UserResponse User,
    string AccessToken
);