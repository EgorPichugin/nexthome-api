using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.QdrantService;

namespace NextHome.Application.Qdrant.Commands;

/// <summary>
/// Request for retrieving similar cards.
/// </summary>
/// <param name="UserId">The ID of the user who made the request.</param>
/// <param name="ChallengeCardId">The ID of the challenge card for which a similar experience card should be found.</param>
public record GetSimilarExperienceCardsRequest(
    Guid UserId,
    Guid ChallengeCardId
);

/// <summary>
/// Command for retrieving similar cards.
/// </summary>
/// <param name="Request">Request fpr retrieving similar cards.</param>
public record GetSimilarExperienceCardsCommand(GetSimilarExperienceCardsRequest Request)
    : IRequest<List<ExperienceCardResponse>>;

/// <summary>
/// Handler for retrieving similar experience cards.
/// </summary>
/// <param name="qdrantService">The service responsible for managing vector database processes.</param>
/// <param name="userRepository">The repository responsible for managing and retrieving user entities.</param>
/// <param name="challengeCardRepository">The repository responsible for managing challenge card entities.</param>
public class GetSimilarExperienceCardsHandler(
    IQdrantService qdrantService,
    IUserRepository userRepository,
    IChallengeCardRepository challengeCardRepository)
    : IRequestHandler<GetSimilarExperienceCardsCommand, List<ExperienceCardResponse>>
{
    /// <inheritdoc />
    public async Task<List<ExperienceCardResponse>> Handle(GetSimilarExperienceCardsCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(command.Request.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        var challengeCard = await challengeCardRepository.GetById(command.Request.ChallengeCardId, cancellationToken);
        if (challengeCard is null)
        {
            throw new ArgumentException("Challenge card not found");
        }

        var similarScoredPointsList = await qdrantService.SearchSimilarCards(card: challengeCard, country: user.Country,
            cancellationToken: cancellationToken);

        if (similarScoredPointsList.Count == 0) return [];

        var cardIds = similarScoredPointsList
            .Where(point => point.Id.HasUuid)
            .Select(point => Guid.Parse(point.Id.Uuid))
            .ToList();
        
        var similarCardEntities = await challengeCardRepository.GetExperienceCardsByIds(cardIds, cancellationToken);

        return similarCardEntities
            .Select(entity => new ExperienceCardResponse(entity.Id, entity.Title, entity.Description))
            .ToList();
    }
}