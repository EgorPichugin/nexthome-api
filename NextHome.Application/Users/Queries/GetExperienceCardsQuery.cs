using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;

namespace NextHome.Application.Users.Queries;

/// <summary>
/// Query to retrieve all user experience cards.
/// </summary>
/// <param name="UserId">The user to whom cards belong to.</param>
public record GetExperienceCardsQuery(
    Guid UserId) : IRequest<List<ExperienceCardResponse>>;

/// <summary>
/// Handlder helps to retrieve user experience cards.
/// </summary>
/// <param name="experienceCardRepository">Services DB experience card requests.</param>
/// <param name="userRepository">Services DB user requests.</param>
public class GetExperienceCardsRequestHandler(IExperienceCardRepository experienceCardRepository, IUserRepository userRepository)
    : IRequestHandler<GetExperienceCardsQuery, List<ExperienceCardResponse>>
{
    /// <inheritdoc />
    public async Task<List<ExperienceCardResponse>> Handle(GetExperienceCardsQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(
            query.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new ArgumentException("User not found");
        }
        
        var cards = await experienceCardRepository.GetExperienceCardsByUserId(user.Id, cancellationToken);

        return cards.Select(card => new ExperienceCardResponse(card.Id, card.Title, card.Description)).ToList();
    }
}