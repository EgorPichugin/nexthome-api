using System.ComponentModel.DataAnnotations;

namespace NextHome.API.Options;

/// <summary>
/// Options for configuring OpenAI services.
/// </summary>
public class OpenAiOptions
{
    /// <summary>
    /// The configuration section name for OpenAI options.
    /// </summary>
    public const string SectionName = "OpenAi";
    
    /// <summary>
    /// The API key for OpenAI services.
    /// </summary>
    [Required(ErrorMessage = "OpenAI key is required")]
    [Key]
    public string Key { get; set; } = string.Empty;
}