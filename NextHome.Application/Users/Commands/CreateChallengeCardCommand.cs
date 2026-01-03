using MediatR;
using NextHome.Application.Common.Exceptions;
using NextHome.Application.Common.Validation;
using NextHome.Application.Users.Interfaces;
using NextHome.Application.Users.Responses;
using NextHome.Core.Entities;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Users.Commands;

/// <summary>
/// Represents a request to create a challenge card associated with a user.
/// This is used to add descriptive cards to a user's profile, containing details
/// like a title and description.
/// </summary>
/// <param name="Title">The title of the challenge card.</param>
/// <param name="Description">The description of the challenge card.</param>
public record CreateChallengeCardRequest(
    string Title,
    string Description
) : ICreateCardRequest;

/// <summary>
/// Command to create a challenge card for a specific user. This command encapsulates the user's identifier
/// and the details of the card to be created, which includes information like title and description.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the challenge card.</param>
/// <param name="Request">An object containing the details of the challenge card to be created.</param>
public record CreateChallengeCardCommand(
    Guid UserId,
    CreateChallengeCardRequest Request) : IRequest<ChallengeCardResponse>;

/// <summary>
/// Handles the process of creating and saving an challenge card for a given user.
/// This includes retrieving the user, validating existence, and persisting the card details
/// in the repository.
/// </summary>
/// <param name="challengeCardRepository">The repository responsible for managing challenge card entities.</param>
/// <param name="userRepository">The repository responsible for managing and retrieving user entities.</param>
/// <param name="cardValidationService">The service that validates card information.</param>
public class CreateChallengeCardHandler(
    IChallengeCardRepository challengeCardRepository,
    IUserRepository userRepository,
    ICardValidationService cardValidationService) : IRequestHandler<CreateChallengeCardCommand, ChallengeCardResponse>
{
    /// <inheritdoc/>
    public async Task<ChallengeCardResponse> Handle(CreateChallengeCardCommand command, CancellationToken cancellationToken)
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
        
        var cardEntity = new ChallengeCardEntity()
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Title = command.Request.Title,
            Description = command.Request.Description,
            CreatedAt = DateTime.UtcNow
        };
        var response = await challengeCardRepository.Add(cardEntity, cancellationToken);
        
        return new ChallengeCardResponse(response.Id, response.Title, response.Description);
    }
}