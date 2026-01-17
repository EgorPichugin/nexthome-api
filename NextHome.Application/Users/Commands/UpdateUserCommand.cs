using MediatR;
using NextHome.Application.Common.Validation;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Users.Responses;
using NextHome.Application.Users.Services;
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
public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Country,
    string City
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
/// Handler for processing the UpdateUserCommand.
/// </summary>
/// <param name="userRepository">User repository for accessing and modifying user data.</param>
/// <param name="userValidationService">Service for validating user data.</param>
/// <param name="userEntityMapper">Mapper for converting between user entities and responses.</param>
public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUserValidationService userValidationService,
    IUserEntityMapper userEntityMapper) : IRequestHandler<UpdateUserCommand, UserResponse>
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
        user.IsProfileCompleted = true;

        await userRepository.Update(user, cancellationToken);
        var response = await userRepository.GetById(user.Id, cancellationToken);

        if (response is null)
        {
            throw new InvalidOperationException("User not found");
        }

        return userEntityMapper.MapUserEntityToResponse(response);
    }
}