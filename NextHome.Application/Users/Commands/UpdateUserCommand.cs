using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Common.Validation;
using NextHome.Application.Common.Exceptions;
using NextHome.Core.Enumerations;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Represents a request to update an existing user's information within the system.
/// </summary>
/// <param name="FirstName">
/// The first name of the user. This is a required parameter.
/// </param>
/// <param name="LastName">
/// The last name of the user. This is a required parameter.
/// </param>
/// <param name="Country">
/// The country of residence of the user. This is a required parameter.
/// </param>
/// <param name="City">
/// The city of residence of the user. This is a required parameter.
/// </param>
/// <param name="Status">
/// The current status of the user within the system, represented as an enumeration. This parameter is optional.
/// </param>
/// <param name="ImmigrationDate">
/// Specifies the date of immigration for the user, if applicable. This parameter is optional.
/// </param>
public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Country,
    string City,
    EStatus? Status = null,
    DateTime? ImmigrationDate = null
);

/// <summary>
/// Represents a command to update an existing user's information in the system.
/// </summary>
/// <param name="UserId">
/// The unique identifier of the user to be updated. This is a required parameter.
/// </param>
/// <param name="Request">
/// The request object containing the user's updated information, such as first name, last name, country,
/// city, status, and immigration date.
/// </param>
public record UpdateUserCommand(
    Guid UserId,
    UpdateUserRequest Request) : IRequest<UserResponse>;

/// <summary>
/// Handles the execution of the UpdateUserCommand, which is responsible for updating
/// an existing user's information in the system.
/// </summary>
/// <param name="userRepository">
/// An instance of <see cref="IUserRepository"/> used to interact with the user data layer.
/// </param>
/// <param name="userValidationService">
/// An instance of <see cref="IUserValidationService"/> used to validate the user's input data
/// before updating the user record.
/// </param>
public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUserValidationService userValidationService) : IRequestHandler<UpdateUserCommand, UserResponse>
{
    /// <inheritdoc/>
    public async Task<UserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var errors = userValidationService.Validate(command.Request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
        
        var user = await userRepository.GetById(
            command.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found");
        }
        
        user.FirstName = command.Request.FirstName;
        user.LastName = command.Request.LastName;
        user.Country = command.Request.Country;
        user.City = command.Request.City;

        await userRepository.Update(user, cancellationToken);
        var response = await userRepository.GetById(user.Id, cancellationToken);

        if (response is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return new UserResponse(
            response.Id,
            response.Email,
            response.FirstName,
            response.LastName,
            response.Country,
            response.City,
            response.Status,
            response.ImmigrationDate,
            response.IsEmailConfirmed
        );
    }
}