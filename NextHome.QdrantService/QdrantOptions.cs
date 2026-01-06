using System.ComponentModel.DataAnnotations;

namespace NextHome.QdrantService;

public class QdrantOptions
{
    public const string Qdrant =  "Qdrant";
    
    /// <summary>
    /// Qdrant database host.
    /// </summary>
    [Required(ErrorMessage = "Host is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string Host { get; set; } = String.Empty;
    
    /// <summary>
    /// Qdrant database port.
    /// </summary>
    [Required(ErrorMessage = "Port is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string Port { get; set; } = String.Empty;
    
    /// <summary>
    /// Qdrant database api key.
    /// </summary>
    [Required(ErrorMessage = "ApiKey is required")]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
    public string ApiKey { get; set; } = String.Empty;
}