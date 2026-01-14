using MediatR;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Command to delete a challenge card for a user.
/// </summary>
/// <param name="UserId">The identifier of the user who owns the card.</param>
/// <param name="CardId">The identifier of the card.</param>
public record DeleteChallengeCardCommand(
    Guid UserId,
    Guid CardId
) : IRequest<Unit>;

/// <summary>
/// Handles the execution of the DeleteChallengeCardCommand, which deletes a challenge card.
/// </summary>
/// <param name="challengeCardRepository">The repository used to access and delete challenge card entities.</param>
/// <param name="userRepository">The repository used to verify the existence of the user associated with the card.</param>
public class DeleteChallengeCardCommandHandler(
    IChallengeCardRepository challengeCardRepository,
    IUserRepository userRepository)
    : IRequestHandler<DeleteChallengeCardCommand, Unit>
{
    /// <inheritdoc/>
    public async Task<Unit> Handle(DeleteChallengeCardCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        var card = await challengeCardRepository.GetById(command.CardId, cancellationToken);
        if (card is null)
        {
            throw new ArgumentException("Card not found");
        }

        await challengeCardRepository.Delete(command.CardId, cancellationToken);

        return Unit.Value;
    }
}