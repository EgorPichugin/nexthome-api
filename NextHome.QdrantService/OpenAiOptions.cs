using System.ComponentModel.DataAnnotations;

namespace NextHome.QdrantService;

public class OpenAiOptions
{
    public const string OpenAi =  "OpenAi";
    
    /// <summary>
    /// OpenAi API Key.
    /// </summary>
    [Required(ErrorMessage = "ApiKey is required")]
    public string Key { get; set; } = String.Empty;
}