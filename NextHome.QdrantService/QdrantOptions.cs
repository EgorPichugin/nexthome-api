using System.ComponentModel.DataAnnotations;

namespace NextHome.QdrantService;

public class QdrantOptions
{
    public const string Qdrant =  "Qdrant";
    
    /// <summary>
    /// Qdrant database host.
    /// </summary>
    [Required(ErrorMessage = "Host is required")]
    public string Host { get; set; } = String.Empty;

    /// <summary>
    /// Qdrant database port.
    /// </summary>
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int Port { get; set; }
}