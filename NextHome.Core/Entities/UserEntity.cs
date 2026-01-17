using NextHome.Core.Enumerations;

namespace NextHome.Core.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class UserEntity
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Authentication identifier of the user.
    /// </summary>
    public required  string AuthId { get; init; }
    
    /// <summary>
    /// Date and time when the user was created.
    /// </summary>
    public required DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// First name of the user.
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Country of the user.
    /// </summary>
    public string? Country { get; set; }
    
    /// <summary>
    /// City of the user.
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// Verify whether the profile completed or not.
    /// </summary>
    public bool IsProfileCompleted { get; set; } = false;
    
    
    /// <summary>
    /// Experience cards associated with the user.
    /// </summary>
    public ICollection<ExperienceCardEntity> ExperienceCards { get; init; } = [];
    
    /// <summary>
    /// Challenge cards associated with the user.
    /// </summary>
    public ICollection<ChallengeCardEntity> ChallengeCards { get; init; } = [];
}
