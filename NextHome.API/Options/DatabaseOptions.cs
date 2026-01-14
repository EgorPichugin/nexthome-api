using System.ComponentModel.DataAnnotations;

namespace NextHome.API.Options;

/// <summary>
/// Configuration options for the database connection.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// The name of the configuration section for database options.
    /// </summary>
    public const string SectionName = "Database";
    
    /// <summary>
    /// The database connection URL.
    /// </summary>
    [Required(ErrorMessage = "Database URL is required")]
    public string Url { get; set; } = string.Empty;
}