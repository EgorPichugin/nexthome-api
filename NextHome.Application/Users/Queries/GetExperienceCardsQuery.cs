using MediatR;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Queries;

public record GetExperienceCardsQuery(
    Guid UserId) : IRequest<List<ExperienceCardResponse>>;

public class GetExperienceCardsRequestHandler(IExperienceCardRepository experienceCardRepository, IUserRepository userRepository)
    : IRequestHandler<GetExperienceCardsQuery, List<ExperienceCardResponse>>
{
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