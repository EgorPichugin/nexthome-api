namespace NextHome.Core.Interfaces.Services;

/// <summary>
/// Interface of the service that is responsible for connection between API and Auth0 server.
/// </summary>
public interface IAuth0Manager
{
    /// <summary>
    /// Get Auth0 access token for communication with Auth0 server.
    /// </summary>
    /// <param name="clientId">The Auth0 application client ID.</param>
    /// <param name="clientSecret">The Auth0 application client secret.</param>
    /// <param name="domain">The Auth0 tenant domain.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The access token.</returns>
    Task<string> GetAccessToken(string clientId, string clientSecret, string domain, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves the email address associated with a specific Auth0 user.
    /// </summary>
    /// <param name="authId">The Auth0 user identifier (sub claim).</param>
    /// <param name="clientId">The Auth0 application client ID.</param>
    /// <param name="clientSecret">The Auth0 application client secret.</param>
    /// <param name="domain">The Auth0 tenant domain.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>The user's email address if found; otherwise, null.</returns>
    Task<string?> GetUserEmailByAuthId(string authId, string clientId, string clientSecret, string domain, CancellationToken cancellationToken);
}