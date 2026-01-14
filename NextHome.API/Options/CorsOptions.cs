using System.ComponentModel.DataAnnotations;

namespace NextHome.API.Options;

/// <summary>
/// Configuration options for CORS (Cross-Origin Resource Sharing).
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// The name of the configuration section for CORS options.
    /// </summary>
    public const string SectionName = "Cors";
    
    /// <summary>
    /// The name of the CORS policy.
    /// </summary>
    [Required(ErrorMessage = "Cors policy name is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string PolicyName { get; set; } = string.Empty;
    
    /// <summary>
    /// The client URL allowed for CORS requests.
    /// </summary>
    [Required(ErrorMessage = "Client URL is required")]
    [Url]
    public string ClientUrl { get; set; } = string.Empty;
}