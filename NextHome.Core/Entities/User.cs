namespace NextHome.Core.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public record User
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Password hash of the user.
    /// </summary>
    public required string PasswordHash { get; init; }
    
    /// <summary>
    /// First name of the user.
    /// </summary>
    public required string FirstName { get; init; }
    
    /// <summary>
    /// Last name of the user.
    /// </summary>
    public required string LastName { get; init; }
    
    /// <summary>
    /// Date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
