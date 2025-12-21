namespace NextHome.API;

/// <summary>
/// Environment manager that is responsible for loading environment variables from .env file
/// </summary>
public static class EnvironmentManager
{
    /// <summary>
    /// CORS policy name.
    /// </summary>
    public static readonly string CorsPolicyName;

    /// <summary>
    /// Client URL.
    /// </summary>
    public static readonly string ClientUrl;

    /// <summary>
    /// Load environment variables from .env file.
    /// </summary>
    static EnvironmentManager()
    {
        DotNetEnv.Env.Load();

        CorsPolicyName = GetRequired("CORS_POLICY_NAME");
        ClientUrl = GetRequired("CLIENT_URL");
    }
    
    /// <summary>
    /// Get required environment variable.
    /// </summary>
    /// <param name="key">The name of environment variable.</param>
    /// <returns>Value from .env file.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static string GetRequired(string key)
    {
        return Environment.GetEnvironmentVariable(key)
               ?? throw new InvalidOperationException($"Environment variable '{key}' is not set.");
    }
}