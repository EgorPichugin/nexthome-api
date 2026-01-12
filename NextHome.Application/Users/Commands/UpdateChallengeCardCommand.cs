using MediatR;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Common.Validation;
using NextHome.Application.Users.Interfaces;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Request to update an existing challenge card.
/// </summary>
/// <param name="Title">The title of the card to update.</param>
/// <param name="Description">The description of the card to update.</param>
public record UpdateChallengeCardRequest(
    string Title,
    string Description
) : IUpdateCardRequest;

/// <summary>
/// Command to update an existing challenge card for a user.
/// </summary>
/// <param name="UserId">The identifier of the user who owns the card.</param>
/// <param name="CardId">The identifier of the challenge card to update.</param>
/// <param name="Request">The update request containing the details of the card to modify.</param>
public record UpdateChallengeCardCommand(
    Guid UserId,
    Guid CardId,
    UpdateChallengeCardRequest Request) : IRequest<ExperienceCardResponse>;

/// <summary>
/// Handles the execution of the UpdateChallengeCardCommand, which updates the details of an existing challenge card.
/// </summary>
/// <param name="challengeCardRepository">The repository used to access and update challenge card entities.</param>
/// <param name="userRepository">The repository used to verify the existence of the user associated with the card.</param>
/// <param name="moderationService">The service responsible for moderating content for compliance.</param>
/// <param name="cardValidationService">The validation service used to validate the update request.</param>
public class UpdateChallengeCardCommandHandler(
    IChallengeCardRepository challengeCardRepository,
    IUserRepository userRepository,
    IModerationService moderationService,
    ICardValidationService cardValidationService)
    : IRequestHandler<UpdateChallengeCardCommand, ExperienceCardResponse>
{
    /// <inheritdoc/>
    public async Task<ExperienceCardResponse> Handle(UpdateChallengeCardCommand command,
        CancellationToken cancellationToken)
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

        var errors = cardValidationService.Validate(command.Request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        await moderationService.Moderate(
            [command.Request.Title, command.Request.Description], 
            cancellationToken
        );

        card.Title = command.Request.Title;
        card.Description = command.Request.Description;

        await challengeCardRepository.Update(card, cancellationToken);

        var response = await challengeCardRepository.GetById(card.Id, cancellationToken);

        if (response is null)
        {
            throw new InvalidOperationException("Card not found");
        }

        return new ExperienceCardResponse(response.Id, response.Title, response.Description);
    }
}