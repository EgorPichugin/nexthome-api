namespace NextHome.API.Options;

/// <summary>
/// Options for configuring Swagger.
/// </summary>
public class SwaggerOptions
{
    /// <summary>
    /// The configuration section name for Swagger options.
    /// </summary>
    public const string SectionName = "Swagger";
    

    /// <summary>
    /// Indicates whether Swagger is enabled.
    /// </summary>
    public bool Enable { get; set; }
}