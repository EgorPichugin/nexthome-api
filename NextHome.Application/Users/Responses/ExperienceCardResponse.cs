namespace NextHome.Application.Users.Responses;

/// <summary>
/// Represents a basic card interface with properties for identification
/// and descriptive information.
/// </summary>
public interface ICard
{
    /// <summary>
    /// Gets the unique identifier for the card.
    /// </summary>
    Guid CardId { get; }
    
    /// <summary>
    /// Gets the summary of the card.
    /// </summary>
    string Title { get; }
    
    /// <summary>
    /// Gets the detailed description of the card.
    /// </summary>
    string Description { get; }
}

/// <inheritdoc cref="ICard"/>
public record ExperienceCardResponse(
    Guid CardId,
    string Title,
    string Description) : ICard;