using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Application.Users.Services;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Queries;

/// <summary>
/// Get all users query.
/// </summary>
public record GetAllUsersQuery : IRequest<List<UserResponse>>;

/// <summary>
/// Handler for GetAllUsersQuery
/// </summary>
/// <param name="userRepository">User repository for accessing user data.</param>
/// <param name="userEntityMapper">Mapper for converting between user entities and responses.</param>
public class GetAllUsersRequestHandler(IUserRepository userRepository, IUserEntityMapper userEntityMapper)
    : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
{
    /// <inheritdoc />
    public async Task<List<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var userEntities = await userRepository.GetAll(cancellationToken) ?? [];
        return userEntityMapper.MapUserEntityToResponse(userEntities);
    }
}