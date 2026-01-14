using MediatR;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Command to delete a user by their email address.
/// </summary>
/// <param name="Email">The email address of the user to be deleted.</param>
public record DeleteUserByEmailCommand(
    string Email) : IRequest;
    
/// <summary>
/// Handler for deleting a user by their email address.
/// </summary>
/// <param name="userRepository">Repository for user data.</param>
public class DeleteUserByEmailHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteUserByEmailCommand>
{
    /// <inheritdoc/>
    public async Task Handle(DeleteUserByEmailCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByEmail(request.Email, ct)
                   ?? throw new InvalidOperationException("User not found");

        await userRepository.Delete(user.Id, ct);
    }
}