using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Enumerations;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Commands;

public record UpdateUserRequest(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Country,
    string City,
    EStatus? Status = null,
    DateTime? ImmigrationDate = null
) : IRequest<UserResponse>;

public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserRequest, UserResponse>
{
    public async Task<UserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Country = request.Country;
        user.City = request.City;

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
            response.ImmigrationDate
        );
    }
}