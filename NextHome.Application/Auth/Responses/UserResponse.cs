using NextHome.Core.Enumerations;

namespace NextHome.Application.Auth.Responses;

/// <summary>
/// Represents the response model for user-related operations within the application.
/// </summary>
/// <param name="UserId">The globally unique identifier of the user.</param>
/// <param name="Email">The email address associated with the user.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Country">The country where the user is based.</param>
/// <param name="City">The city where the user resides. This field is optional.</param>
/// <param name="Status">The status of the user, defined by the <c>EStatus</c> enumeration. This field is optional.</param>
/// <param name="ImmigrationDate">The date when the user immigrated. This field is optional.</param>
public record UserResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Country,
    string City,
    EStatus? Status,
    DateTime? ImmigrationDate
);
