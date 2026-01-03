using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Queries;

/// <summary>
/// Query to retrieve user challenge cards.
/// </summary>
/// <param name="UserId">The user to whom cards belong to.</param>
public record GetChallengeCardsQuery(
    Guid UserId) : IRequest<List<ChallengeCardResponse>>;

/// <summary>
/// Handler for retrieving user challenge cards.
/// </summary>
/// <param name="challengeCardRepository">Services DB challenge card requests.</param>
/// <param name="userRepository">Services DB user requests.</param>
public class GetChallengeCardsRequestHandler(IChallengeCardRepository challengeCardRepository, IUserRepository userRepository)
    : IRequestHandler<GetChallengeCardsQuery, List<ChallengeCardResponse>>
{
    /// <inheritdoc />
    public async Task<List<ChallengeCardResponse>> Handle(GetChallengeCardsQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(
            query.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found");
        }
        
        var cards = await challengeCardRepository.GetChallengeCardsByUserId(user.Id, cancellationToken);

        return cards.Select(card => new ChallengeCardResponse(card.Id, card.Title, card.Description)).ToList();
    }
}