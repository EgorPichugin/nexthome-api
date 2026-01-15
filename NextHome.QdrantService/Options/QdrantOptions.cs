using System.ComponentModel.DataAnnotations;
using NextHome.Core.Interfaces;

namespace NextHome.QdrantService.Options;

/// <summary>
/// Configuration options for Qdrant service.
/// </summary>
public class QdrantOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName =  "Qdrant";
    
    /// <summary>
    /// The host address of the Qdrant service.
    /// </summary>
    [Required(ErrorMessage = "Host is required")]
    public string Host { get; set; } = String.Empty;

    /// <summary>
    /// The port number of the Qdrant service.
    /// </summary>
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
    public int Port { get; set; }
}