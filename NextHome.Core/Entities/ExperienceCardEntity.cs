namespace NextHome.Core.Entities;

/// <summary>
/// Represents an experience card.
/// </summary>
public record ExperienceCardEntity
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
    public required string Title { get; init; }

    /// <summary>
    /// Gets the detailed description of the experience card.
    /// </summary>
    public required string Description { get; init; }
}