using NextHome.Application.Users.Responses;
using NextHome.Core.Entities;

namespace NextHome.Application.Users.Services;

/// <summary>
/// Mapper interface for UserEntity to UserResponse
/// </summary>
public interface IUserEntityMapper
{
    /// <summary>
    /// Map UserEntity to UserResponse
    /// </summary>
    /// <param name="user">The user entity to be mapped.</param>
    /// <returns>A UserResponse object representing the mapped user entity.</returns>
    UserResponse MapUserEntityToResponse(UserEntity user);
    
    /// <summary>
    /// Map list of UserEntity to a list of UserResponse
    /// </summary>
    /// <param name="users">The list of user entities to be mapped.</param>
    /// <returns>A list of UserResponse objects representing the mapped user entities.</returns>
    List<UserResponse> MapUserEntityToResponse(List<UserEntity> users);
}

/// <summary>
/// Implementation of IUserEntityMapper
/// </summary>
public class UserEntityMapper : IUserEntityMapper
{
    /// <inheritdoc />
    public UserResponse MapUserEntityToResponse(UserEntity user)
    {
        return new UserResponse(
            UserId: user.Id,
            Email: user.Email,
            IsCompleted: user.IsProfileCompleted,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Country: user.Country,
            City: user.City,
            AvatarUrl: user.AvatarUrl);
    }

    /// <inheritdoc />
    public List<UserResponse> MapUserEntityToResponse(List<UserEntity> users)
    {
        return users.Select(MapUserEntityToResponse).ToList();
    }
}