using MediatR;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Common.Validation;
using NextHome.Application.Users.Interfaces;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;
using NextHome.Core.Interfaces.Repositories;
using NextHome.Core.Interfaces.Services;
using NextHome.QdrantService;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Request to update an existing experience card.
/// </summary>
/// <param name="Title">The title of the card to update.</param>
/// <param name="Description">The description of the card to update.</param>
public record UpdateExperienceCardRequest(
    string Title,
    string Description
) : IUpdateCardRequest;

/// <summary>
/// Command to update an existing experience card for a user.
/// </summary>
/// <param name="UserId">The identifier of the user who owns the card.</param>
/// <param name="CardId">The identifier of the experience card to update.</param>
/// <param name="Request">The update request containing the details of the card to modify.</param>
public record UpdateExperienceCardCommand(
    Guid UserId,
    Guid CardId,
    UpdateExperienceCardRequest Request) : IRequest<ExperienceCardResponse>;

/// <summary>
/// Handles the execution of the UpdateExperienceCardCommand, which updates the details of an existing experience card.
/// </summary>
/// <param name="experienceCardRepository">The repository used to access and update experience card entities.</param>
/// <param name="userRepository">The repository used to verify the existence of the user associated with the card.</param>
/// <param name="cardValidationService">The validation service used to validate the update request.</param>
/// <param name="moderationService">The service responsible for moderating content for compliance.</param>
/// <param name="qdrantService">The service is responsible for communication with Qdrant.</param>
public class UpdateExperienceCardCommandHandler(
    IExperienceCardRepository experienceCardRepository,
    IUserRepository userRepository,
    ICardValidationService cardValidationService,
    IModerationService moderationService,
    IQdrantService qdrantService)
    : IRequestHandler<UpdateExperienceCardCommand, ExperienceCardResponse>
{
    /// <inheritdoc/>
    public async Task<ExperienceCardResponse> Handle(UpdateExperienceCardCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(command.UserId, cancellationToken) 
            ?? throw new ArgumentException("User not found");

        var card = await experienceCardRepository.GetById(command.CardId, cancellationToken) 
            ?? throw new ArgumentException("Card not found");

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

        await experienceCardRepository.Update(card, cancellationToken);

        var response = await experienceCardRepository.GetById(card.Id, cancellationToken);
        if (response is null)
        {
            throw new InvalidOperationException("Card not found");
        }

        await qdrantService.SaveExperienceCard(response, user.Country,cancellationToken: cancellationToken);

        return new ExperienceCardResponse(response.Id, response.Title, response.Description);
    }
}