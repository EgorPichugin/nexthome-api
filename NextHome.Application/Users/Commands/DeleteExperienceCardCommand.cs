using MediatR;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;
using NextHome.QdrantService;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Command to delete an experience card for a user.
/// </summary>
/// <param name="UserId">The identifier of the user who owns the card.</param>
/// <param name="CardId">The identifier of the card.</param>
public record DeleteExperienceCardCommand(
    Guid UserId,
    Guid CardId
) : IRequest<Unit>;

/// <summary>
/// Handles the execution of the DeleteExperienceCardCommand, which deletes an experience card.
/// </summary>
/// <param name="experienceCardRepository">The repository used to access and delete experience card entities.</param>
/// <param name="userRepository">The repository used to verify the existence of the user associated with the card.</param>
/// <param name="qdrantService">The service is responsible for communication with Qdrant.</param>
public class DeleteExperienceCardCommandHandler(
    IExperienceCardRepository experienceCardRepository,
    IUserRepository userRepository,
    IQdrantService qdrantService)
    : IRequestHandler<DeleteExperienceCardCommand, Unit>
{
    /// <inheritdoc/>
    public async Task<Unit> Handle(DeleteExperienceCardCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        var card = await experienceCardRepository.GetById(command.CardId, cancellationToken);
        if (card is null)
        {
            throw new ArgumentException("Card not found");
        }

        await experienceCardRepository.Delete(command.CardId, cancellationToken);
        await qdrantService.DeleteExperienceCard(card: card, collectionName: null, cancellationToken:  cancellationToken);

        return Unit.Value;
    }
}