using MediatR;
using NextHome.Application.Auth.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Queries;

public record GetAllUsersQuery: IRequest<List<UserResponse>>;

public class GetAllUsersRequestHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var userEntities = await userRepository.GetAll(cancellationToken) ?? [];
        return [.. userEntities.Select(user => new UserResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Country,
            user.City,
            user.Status,
            user.ImmigrationDate,
            user.IsEmailConfirmed
        ))];
    }
}