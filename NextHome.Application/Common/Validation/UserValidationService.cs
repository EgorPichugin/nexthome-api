using System.Net.Mail;
using NextHome.Application.Auth.Commands;
using NextHome.Application.Users.Commands;

namespace NextHome.Application.Common.Validation;

/// <summary>
/// Defines a service for validating user information provided in user-related operations.
/// </summary>
public interface IUserValidationService
{
    /// <summary>
    /// Validates the user information provided in the specified request.
    /// </summary>
    /// <param name="userRequest">The request containing user information to be validated.</param>
    /// <returns>A list of validation error messages. If the list is empty, the user information is considered valid.</returns>
    List<string> Validate(RegisterUserRequest userRequest);

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
    public List<string> Validate(RegisterUserRequest userRequest)
    {
        var errors = new List<string>();

        if (!IsValidName(userRequest.FirstName))
            errors.Add("Invalid first name.");

        if (!IsValidSurname(userRequest.LastName))
            errors.Add("Invalid last name.");

        if (!IsValidEmail(userRequest.Email))
            errors.Add("Invalid email address.");

        if (!IsValidPassword(userRequest.Password))
            errors.Add("Password must be at least 8 characters long.");

        if (!IsValidCountry(userRequest.Country))
            errors.Add("Invalid country.");

        return errors;
    }
    
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
    
    private static bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private static bool IsValidSurname(string surname)
    {
        return !string.IsNullOrWhiteSpace(surname);
    }

    private static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        return password.Length >= 8;
    }

    private static bool IsValidCountry(string country)
    {
        return !string.IsNullOrWhiteSpace(country);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try
        {
            var trimmedEmail = email.Trim();
            var address = new MailAddress(trimmedEmail);
            return address.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}