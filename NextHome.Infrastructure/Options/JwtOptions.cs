using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NextHome.Core.Interfaces;

namespace NextHome.Infrastructure.Options;

/// <summary>
/// Configuration options for JWT.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// The secret key used for JWT signing.
    /// </summary>
    [Required(ErrorMessage = "JWT secret is required")]
    [Key]
    public string Secret { get; set; } = string.Empty;
}