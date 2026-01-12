using System.ComponentModel.DataAnnotations;

namespace NextHome.API;

/// <summary>
/// Defines environment options.
/// </summary>
public class EnvironmentOptions
{
    public const string Environment = "Environment";

    /// <summary>
    /// Cors policy name.
    /// </summary>
    [Required(ErrorMessage = "Cors policy name is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string CorsPolicyName { get; set; } = string.Empty;

    /// <summary>
    /// Client URL for cors policy.
    /// </summary>
    [Required(ErrorMessage = "Client URL is required")]
    [Url]
    public string ClientUrl { get; set; } = string.Empty;

    /// <summary>
    /// Database URL.
    /// </summary>
    [Required(ErrorMessage = "Database URL is required")]
    public string DatabaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Defines whether swagger should be enabled or not.
    /// </summary>
    public bool EnableSwagger { get; set; }
}