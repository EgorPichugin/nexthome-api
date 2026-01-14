using System.ComponentModel.DataAnnotations;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Options;

/// <summary>
/// Configuration options for the API.
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Section name in configuration.
    /// </summary>
    public const string SectionName = "Api";

    /// <summary>
    /// The base URL of the API.
    /// </summary>
    [Required(ErrorMessage = "API URL is required")]
    [Url]
    public string BaseUrl { get; set; } = string.Empty;
}