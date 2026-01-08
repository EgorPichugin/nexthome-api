namespace NextHome.Core.Entities;

/// <summary>
/// Represents a challenge card.
/// </summary>
public class ChallengeCardEntity : ICardEntity
{
    /// <summary>
    /// Gets the unique identifier for the experience card.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the user id associated with the experience card.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Gets the user associated with the experience card.
    /// </summary>
    public required UserEntity User { get; init; }

    /// <summary>
    /// Gets the title of the experience card.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets the detailed description of the experience card.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Date and time when the card was created.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}