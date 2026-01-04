namespace NextHome.API;

/// <summary>
/// Defines environment options.
/// </summary>
public class EnvironmentOptions
{
    /// <summary>
    /// Cors policy name.
    /// </summary>
    public string CORS_POLICY_NAME { get; set; } = default!;
    
    /// <summary>
    /// Client URL for cors policy.
    /// </summary>
    public string CLIENT_URL { get; set; } = default!;
    
    /// <summary>
    /// Database URL.
    /// </summary>
    public string DATABASE_URL { get; set; } = default!;
    
    /// <summary>
    /// Defines whether swagger should be enabled or not.
    /// </summary>
    public bool ENABLE_SWAGGER { get; set; }

}