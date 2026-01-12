using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Application.Common.Validation;
using NextHome.Application.Common.Exceptions;
using NextHome.Core.Enumerations;

namespace NextHome.Application.Auth.Commands;

/// <summary>
/// Represents the request to register a new user.
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The password for the user account.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
/// <param name="Country">The country to where the user immigrated.</param>
/// <param name="City">The city where the user lives.</param>
/// <param name="Status">The immigrant status.</param>
/// <param name="ImmigrationDate">The immigration date.</param>
public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Country,
    string City,
    EStatus? Status = null,
    DateTime? ImmigrationDate = null
);

/// <summary>
/// Represents a command to register a new user.
/// </summary>
/// <param name="Request">The request containing the details for user registration.</param>
public record RegisterUserCommand(
    RegisterUserRequest Request
) : IRequest<UserResponse>;

/// <summary>
/// Represents the command handler to register a new user.
/// </summary>
public class RegisterCommandHandler(IUserRepository userRepository, IUserValidationService userValidationService) : IRequestHandler<RegisterUserCommand, UserResponse>
{
    /// <inheritdoc/>
    public async Task<UserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var errors = userValidationService.Validate(command.Request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        if (await userRepository.Exists(command.Request.Email, cancellationToken))
        {
            throw new InvalidOperationException($"User with email {command.Request.Email} already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Request.Password);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = command.Request.Email.Trim().ToLower(),
            PasswordHash = passwordHash,
            FirstName = command.Request.FirstName.Trim(),
            LastName = command.Request.LastName.Trim(),
            Country = command.Request.Country.Trim(),
            City = command.Request.City.Trim(),
            Status = command.Request.Status,
            ImmigrationDate = command.Request.ImmigrationDate,
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
}