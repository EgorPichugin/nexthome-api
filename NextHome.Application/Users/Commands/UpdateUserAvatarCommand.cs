using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces.Repositories;
using NextHome.Application.Users.Services;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Request to update user avatar.
/// </summary>
/// <param name="AvatarUrl">Avatart Url.</param>
public record UpdateUserAvatarRequest(string AvatarUrl);

/// <summary>
/// Command to update user avatar.
/// </summary>
/// <param name="UserId">The ID of the user to update avatar</param>
/// <param name="Request">The <see cref="UpdateUserAvatarRequest"/> isntance.</param>
public record UpdateUserAvatarCommand(Guid UserId, UpdateUserAvatarRequest Request) : IRequest<UserResponse>;

/// <summary>
/// Handler to update user avatar.
/// </summary>
/// <param name="userRepository">User repository.</param>
/// <param name="userEntityMapper">Service to map user entity to response.</param>
public class UpdateUserAvatarHandler(IUserRepository userRepository, IUserEntityMapper userEntityMapper) : IRequestHandler<UpdateUserAvatarCommand, UserResponse>
{
    public async Task<UserResponse> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        if (Uri.TryCreate(request.Request.AvatarUrl, UriKind.Absolute, out var avatarUri))
        {
            user.AvatarUrl = avatarUri.ToString();
        }
        else
        {
            throw new ArgumentException("Invalid avatar URL");
        }

        await userRepository.Update(user, cancellationToken);

        return userEntityMapper.MapUserEntityToResponse(user);
    }
}