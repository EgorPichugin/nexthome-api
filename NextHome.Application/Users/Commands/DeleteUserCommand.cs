using MediatR;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;
using NextHome.QdrantService;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Represents a command to delete a user.
/// </summary>
/// <param name="UserId">The ID of the user that should be deleted.</param>
public record DeleteUserCommand(
    Guid UserId) : IRequest;

/// <summary>
/// Handles the deletion of a user.
/// </summary>
/// <param name="userRepository">Repository for user data.</param>
/// <param name="experienceCardRepository">Repository for experience card data.</param>
/// <param name="challengeCardRepository">Repository for challenge card data.</param>
/// <param name="qdrantService">Service for interacting with Qdrant.</param>
public class DeleteUserHandler(
    IUserRepository userRepository,
    IExperienceCardRepository experienceCardRepository,
    IChallengeCardRepository challengeCardRepository,
    IQdrantService qdrantService) : IRequestHandler<DeleteUserCommand>
{
    /// <inheritdoc/>
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        var experienceCards =
            await experienceCardRepository.GetExperienceCardsByUserId(request.UserId, cancellationToken);
        if (experienceCards.Count > 0)
        {
            await DeleteCards(
                cards: experienceCards,
                deleteFromDb: experienceCardRepository.Delete,
                (experienceCards, cancellationToken) =>
                    qdrantService.DeleteExperienceCards(experienceCards, collectionName: null, cancellationToken),
                cancellationToken);
        }

        var challengeCards = await challengeCardRepository.GetChallengeCardsByUserId(request.UserId, cancellationToken);
        if (challengeCards.Count > 0)
        {
            await DeleteCards(
                cards: challengeCards,
                deleteFromDb: challengeCardRepository.Delete,
                deleteFromQdrant: null,
                cancellationToken);
        }

        await userRepository.Delete(request.UserId, cancellationToken);
    }

    /// <summary>
    /// Deletes cards from both the database and Qdrant (if applicable).
    /// </summary>
    /// <typeparam name="T">The type of card entity.</typeparam>
    /// <param name="cards">The collection of cards to be deleted.</param>
    /// <param name="deleteFromDb">Function to delete cards from the database.</param>
    /// <param name="deleteFromQdrant">Function to delete cards from Qdrant.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    private async Task DeleteCards<T>(
        IReadOnlyCollection<T> cards,
        Func<Guid, CancellationToken, Task> deleteFromDb,
        Func<IReadOnlyCollection<T>, CancellationToken, Task>? deleteFromQdrant,
        CancellationToken cancellationToken)
        where T : ICardEntity
    {
        if (cards.Count == 0) return;

        if (deleteFromQdrant is not null)
        {
            await deleteFromQdrant(cards, cancellationToken);
        }

        foreach (var card in cards)
        {
            await deleteFromDb(card.Id, cancellationToken);
        }
    }
}