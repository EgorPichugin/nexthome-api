using NextHome.Application.Users.Commands;

namespace NextHome.Application.Common.Validation;

/// <summary>
/// Defines a service for validating user information provided in user-related operations.
/// </summary>
public interface IUserValidationService
{
    /// <summary>
    /// Validates the user information provided in the specified update request.
    /// </summary>
    /// <param name="userRequest">The request containing updated user information to be validated.</param>
    /// <returns>A list of validation error messages. If the list is empty, the updated user information is considered valid.</returns>
    List<string> Validate(UpdateUserRequest userRequest);
}

/// <inheritdoc />
public class UserValidationService : IUserValidationService
{
    /// <inheritdoc />
    public List<string> Validate(UpdateUserRequest userRequest)
    {
        var errors = new List<string>();

        if (!IsValidName(userRequest.FirstName))
            errors.Add("Invalid first name.");

        if (!IsValidSurname(userRequest.LastName))
            errors.Add("Invalid last name.");

        if (!IsValidCountry(userRequest.Country))
            errors.Add("Invalid country.");

        return errors;
    }

    /// <summary>
    /// Determines whether the specified name is valid.
    /// </summary>
    /// <param name="name">The name to validate.</param>
    /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
    private static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    /// <summary>
    /// Determines whether the specified surname is valid.
    /// </summary>
    /// <param name="surname">The surname to validate.</param>
    /// <returns>A boolean value indicating whether the surname is valid. Returns true if the surname is non-empty and not whitespace; otherwise, false.</returns>
    private static bool IsValidSurname(string surname)
    {
        return !string.IsNullOrWhiteSpace(surname);
    }

    /// <summary>
    /// Determines whether the specified country is considered valid.
    /// </summary>
    /// <param name="country">The country to be validated.</param>
    /// <returns>True if the country is valid; otherwise, false.</returns>
    private static bool IsValidCountry(string country)
    {
        return !string.IsNullOrWhiteSpace(country);
    }
}