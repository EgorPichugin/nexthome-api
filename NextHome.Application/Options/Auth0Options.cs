using System.ComponentModel.DataAnnotations;

namespace NextHome.Application.Options;

/// <summary>
/// Auth0 options.
/// </summary>
public class Auth0Options
{
    /// <summary>
    /// Auth0 section name.
    /// </summary>
    public const string SectionName = "Auth0";
    
    /// <summary>
    /// Auth0 authority.
    /// </summary>
    [Required(ErrorMessage = "Auth authority url is required")]
    [Url]
    public string Authority { get; set; } = string.Empty;
    
    /// <summary>
    /// Auth0 audience.
    /// </summary>
    [Required(ErrorMessage = "Auth audience url is required")]
    [Url]
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// Auth0 domain url.
    /// </summary>
    [Required(ErrorMessage = "Auth domain url is required")]
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// Auth0 API client ID.
    /// </summary>
    [Required(ErrorMessage = "Auth0 machine to machine client ID is required")]
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Auth0 API client secret.
    /// </summary>
    [Required(ErrorMessage = "Auth0 machine to machine client secret is required")]
    public string ClientSecret { get; set; } = string.Empty;
}