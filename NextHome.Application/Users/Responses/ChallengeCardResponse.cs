namespace NextHome.Application.Users.Responses;

/// <inheritdoc cref="ICard"/>
public record ChallengeCardResponse(
    Guid CardId,
    string Title,
    string Description
) : ICard;