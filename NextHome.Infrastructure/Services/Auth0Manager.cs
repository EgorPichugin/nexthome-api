using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using NextHome.Core.Interfaces.Services;

namespace NextHome.Infrastructure.Services;

/// <summary>
/// Service that is responsible for communication between API and Auth0.
/// </summary>
/// <param name="httpClient"></param>
public class Auth0Manager(HttpClient httpClient) : IAuth0Manager
{
    /// <inheritdoc />
    public async Task<string> GetAccessToken(string clientId, string clientSecret, string domain,
        CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"https://{domain}/oauth/token",
            new
            {
                client_id = clientId,
                client_secret = clientSecret,
                audience = $"https://{domain}/api/v2/",
                grant_type = "client_credentials"
            },
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
        return json.GetProperty("access_token").GetString()!;
    }

    /// <inheritdoc />
    public async Task<string?> GetUserEmailByAuthId(string authId, string clientId, string clientSecret, string domain,
        CancellationToken cancellationToken)
    {
        var token = await GetAccessToken(clientId, clientSecret, domain, cancellationToken);

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://{domain}/api/v2/users/{authId}"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);
        return json.GetProperty("email").GetString();
    }
}