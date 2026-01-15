using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Application.Common.Validation;
using NextHome.Application.Common.Exceptions;
using NextHome.Core.Enumerations;
using Microsoft.Extensions.Options;
using NextHome.Application.Options;
using NextHome.Core.Interfaces.Repositories;
using NextHome.Core.Interfaces.Services;

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
/// Handles the registration of a new user.
/// </summary>
/// <param name="userRepository">Repository for user data operations.</param>
/// <param name="userValidationService">Service for validating user data.</param>
/// <param name="tokenGenerator">Service for generating authentication tokens.</param>
/// <param name="apiOptions">API configuration options.</param>
/// <param name="emailSender">Service is responsible for sending emails.</param>
public class RegisterCommandHandler(IUserRepository userRepository, 
IUserValidationService userValidationService, ITokenGenerator tokenGenerator,
IOptions<ApiOptions> apiOptions,
IEmailSender emailSender) : IRequestHandler<RegisterUserCommand, UserResponse>
{
    /// <inheritdoc/>
    public async Task<UserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var errors = userValidationService.Validate(command.Request);
        if (errors.Count != 0)
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
            CreatedAt = DateTime.UtcNow,
            IsEmailConfirmed = false
        };

        var token = tokenGenerator.GenerateToken();
        var hashedToken = tokenGenerator.HashToken(token);

        user.EmailConfirmationToken = hashedToken;
        user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);

        var link = $"{apiOptions.Value.BaseUrl}/users/confirm-email?token={Uri.EscapeDataString(token)}";
        await emailSender.SendEmail(user.Email, "Confirm your registration", $"Confirm you account: \n{link}");

        var userEntity = await userRepository.Add(user, cancellationToken);

        return new UserResponse(
            userEntity.Id,
            userEntity.Email,
            userEntity.FirstName,
            userEntity.LastName,
            userEntity.Country,
            userEntity.City,
            userEntity.Status,
            userEntity.ImmigrationDate,
            userEntity.IsEmailConfirmed
        );
    }
}