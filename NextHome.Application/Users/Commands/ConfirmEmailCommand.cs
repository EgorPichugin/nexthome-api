using MediatR;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Command to confirm a user's email address.
/// </summary>
/// <param name="Token">The email confirmation token.</param>
public record ConfirmEmailCommand(
    string Token) :  IRequest;

/// <summary>
/// Handler for confirming a user's email address.
/// </summary>
/// <param name="userRepository">Repository for user data.</param>
/// <param name="tokenGenerator">Service for generating and hashing tokens.</param>
public class ConfirmEmailHandler(IUserRepository userRepository, ITokenGenerator tokenGenerator)
    : IRequestHandler<ConfirmEmailCommand>
{
    /// <inheritdoc/>
    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var tokenHash = tokenGenerator.HashToken(request.Token);

        var user = await userRepository.GetByEmailConfirmationToken(tokenHash, cancellationToken)
                   ?? throw new InvalidOperationException("Invalid token");

        if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Token expired");
        }

        if (user.IsEmailConfirmed)
        {
            return;
        }

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        user.EmailConfirmationTokenExpiry = null;

        await userRepository.Update(user, cancellationToken);
    }
}
