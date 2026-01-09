using MediatR;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Common.Validation;
using NextHome.Application.Users.Interfaces;
using NextHome.Application.Users.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;
using NextHome.QdrantService;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Represents a request to create an experience card associated with a user.
/// This is used to add descriptive cards to a user's profile, containing details
/// like a title and description.
/// </summary>
/// <param name="Title">The title of the experience card.</param>
/// <param name="Description">The description of the experience card.</param>
public record CreateExperienceCardRequest(
    string Title,
    string Description
) : ICreateCardRequest;

/// <summary>
/// Command to create an experience card for a specific user. This command encapsulates the user's identifier
/// and the details of the experience card to be created, which includes information like title and description.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the experience card.</param>
/// <param name="Request">An object containing the details of the experience card to be created.</param>
public record CreateExperienceCardCommand(
    Guid UserId,
    CreateExperienceCardRequest Request) : IRequest<ExperienceCardResponse>;

/// <summary>
/// Handles the process of creating and saving an experience card for a given user.
/// This includes retrieving the user, validating existence, and persisting the card details
/// in the repository.
/// </summary>
/// <param name="experienceCardRepository">The repository responsible for managing experience card entities.</param>
/// <param name="userRepository">The repository responsible for managing and retrieving user entities.</param>
/// <param name="cardValidationService">The service that validates experience card information.</param>
/// <param name="qdrantService">The service that communicate with a qdrant vector database.</param>
public class CreateExperienceCardHandler(
    IExperienceCardRepository experienceCardRepository,
    IUserRepository userRepository,
    ICardValidationService cardValidationService,
    IQdrantService qdrantService) : IRequestHandler<CreateExperienceCardCommand, ExperienceCardResponse>
{
    /// <inheritdoc/>
    public async Task<ExperienceCardResponse> Handle(CreateExperienceCardCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetById(command.UserId, cancellationToken);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        var errors = cardValidationService.Validate(command.Request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }

        var cardEntity = new ExperienceCardEntity
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Title = command.Request.Title,
            Description = command.Request.Description,
            CreatedAt = DateTime.UtcNow
        };
        var response = await experienceCardRepository.Add(cardEntity, cancellationToken);
        await qdrantService.SaveExperienceCard(card: cardEntity, country: user.Country,
            cancellationToken: cancellationToken);

        return new ExperienceCardResponse(response.Id, response.Title, response.Description);
    }
}