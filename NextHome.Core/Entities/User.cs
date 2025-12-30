using NextHome.Core.Enumerations;

namespace NextHome.Core.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Password hash of the user.
    /// </summary>
    public required string PasswordHash { get; init; }
    
    /// <summary>
    /// First name of the user.
    /// </summary>
    public required string FirstName { get; set; }
    
    /// <summary>
    /// Last name of the user.
    /// </summary>
    public required string LastName { get; set; }
    
    /// <summary>
    /// Country of the user.
    /// </summary>
    public required string Country { get; set; }
    
    /// <summary>
    /// City of the user.
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// User status in country.
    /// </summary>
    public EStatus? Status { get; set; }
    
    /// <summary>
    /// Date of immigration of the user.
    /// </summary>
    public DateTime? ImmigrationDate { get; set; }
    
    /// <summary>
    /// Date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Experience cards associated with the user.
    /// </summary>
    public ICollection<ExperienceCard> ExperienceCards { get; init; } = [];
}
