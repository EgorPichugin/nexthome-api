using NextHome.Core.Enumerations;

namespace NextHome.Application.Users.Responses;

/// <summary>
/// Represents the response model for user-related operations within the application.
/// </summary>
/// <param name="UserId">The globally unique identifier of the user.</param>
/// <param name="Email">The email address associated with the user.</param>
/// <param name="IsCompleted">Verify whether the profile completed with data.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Country">The country where the user is based.</param>
/// <param name="City">The city where the user resides. This field is optional.</param>
/// <param name="AvatarUrl">The user avatar url.</param>
public record UserResponse(
    Guid UserId,
    string Email,
    bool IsCompleted,
    string? FirstName,
    string? LastName,
    string? Country,
    string? City,
    string? AvatarUrl
);
