using MediatR;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Common.Validation;
using NextHome.Application.Users.Responses;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Request to update an existing experience card.
/// </summary>
/// <param name="Title">The title of the card to update.</param>
/// <param name="Description">The description of the card to update.</param>
public record UpdateExperienceCardRequest(
    string Title,
    string Description);

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
/// <param name="experienceCardValidationService">The validation service used to validate the update request.</param>
public class UpdateExperienceCardCommandHandler(
    IExperienceCardRepository experienceCardRepository,
    IUserRepository userRepository,
    IExperienceCardValidationService experienceCardValidationService)
    : IRequestHandler<UpdateExperienceCardCommand, ExperienceCardResponse>
{
    /// <inheritdoc/>
    public async Task<ExperienceCardResponse> Handle(UpdateExperienceCardCommand command, CancellationToken cancellationToken)
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
        
        var errors = experienceCardValidationService.Validate(command.Request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        card.Title = command.Request.Title;
        card.Description = command.Request.Description;
        
        await experienceCardRepository.Update(card, cancellationToken);

        var response = await experienceCardRepository.GetById(card.Id, cancellationToken);
        
        if (response is null)
        {
            throw new InvalidOperationException("Card not found");
        }
        
        return new ExperienceCardResponse(response.Id, response.Title, response.Description);
    }
}