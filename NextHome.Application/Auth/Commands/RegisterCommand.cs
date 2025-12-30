using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure;
using System.Net.Mail;
using NextHome.Application.Common.Exceptions;
using NextHome.Core.Enumerations;

namespace NextHome.Application.Auth.Commands;

/// <summary>
/// Represents the command to register a new user.
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The password for the user account.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Country">The country to where the user immigrated.</param>
/// <param name="City">The city where the user lives.</param>
/// <param name="Status">The immigrant status.</param>
/// <param name="ImmigrationDate">The immigration date.</param>
public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Country,
    string City,
    EStatus? Status = null,
    DateTime? ImmigrationDate = null
) : IRequest<UserResponse>;

/// <summary>
/// Represents the command handler to register a new user.
/// </summary>
public class RegisterCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterCommand, UserResponse>
{
    /// <inheritdoc/>
    public async Task<UserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var errors = ValidateRequest(request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        if (await userRepository.Exists(request.Email, cancellationToken))
        {
            throw new InvalidOperationException($"User with email {request.Email} already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.Trim(),
            PasswordHash = passwordHash,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Country = request.Country.Trim(),
            City = request.City?.Trim(),
            Status = request.Status,
            ImmigrationDate = request.ImmigrationDate,
            CreatedAt = DateTime.UtcNow
        };

        var userEntity = await userRepository.Add(user, cancellationToken);

        return new UserResponse(
            userEntity.Id,
            userEntity.Email,
            userEntity.FirstName,
            userEntity.LastName,
            userEntity.Country,
            userEntity.City,
            userEntity.Status,
            userEntity.ImmigrationDate
        );
    }

    private static List<string> ValidateRequest(RegisterCommand request)
    {
        var errors = new List<string>();

        if (!IsValidName(request.FirstName))
            errors.Add("Invalid first name.");

        if (!IsValidSurname(request.LastName))
            errors.Add("Invalid last name.");

        if (!IsValidEmail(request.Email))
            errors.Add("Invalid email address.");

        if (!IsValidPassword(request.Password))
            errors.Add("Password must be at least 8 characters long.");

        if (!IsValidCountry(request.Country))
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