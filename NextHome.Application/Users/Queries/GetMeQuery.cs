using MediatR;
using Microsoft.Extensions.Options;
using NextHome.Application.Options;
using NextHome.Application.Users.Responses;
using NextHome.Application.Users.Services;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces.Repositories;
using NextHome.Core.Interfaces.Services;

namespace NextHome.Application.Users.Queries;

/// <summary>
/// Get current user info
/// </summary>
/// <param name="AuthId">The authentication ID of the current user.</param>
public record GetMeQuery(string AuthId) : IRequest<UserResponse>;

/// <summary>
/// Handler for GetMeQuery
/// </summary>
/// <param name="userRepository">User repository for accessing user data.</param>
/// <param name="userEntityMapper">Mapper for converting between user entities and responses.</param>
public class GetMeRequestHandler(
    IUserRepository userRepository,
    IUserEntityMapper userEntityMapper,
    IAuth0Manager auth0Manager,
    IOptions<Auth0Options> authOptions)
    : IRequestHandler<GetMeQuery, UserResponse>
{
    /// <inheritdoc />
    public async Task<UserResponse> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByAuthId(request.AuthId, cancellationToken);

        if (user is not null) return userEntityMapper.MapUserEntityToResponse(user);

        var email = await auth0Manager.GetUserEmailByAuthId(request.AuthId, authOptions.Value.ClientId,
            authOptions.Value.ClientSecret, authOptions.Value.Domain, cancellationToken);
        
        user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = email ?? string.Empty,
            AuthId = request.AuthId,
            CreatedAt = DateTime.UtcNow,
        };
        await userRepository.Add(user, cancellationToken);

        return userEntityMapper.MapUserEntityToResponse(user);
    }
}