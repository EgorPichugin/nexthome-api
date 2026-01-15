using NextHome.Core.Exceptions;
using NextHome.Core.Interfaces.Services;
using OpenAI.Moderations;

namespace NextHome.Infrastructure.Services;

/// <inheritdoc/>
public class ModerationService(ModerationClient moderationClient) : IModerationService
{
    /// <inheritdoc/>
    public async Task Moderate(string content, CancellationToken cancellationToken = default)
    {
        var response = await moderationClient.ClassifyTextAsync(content, cancellationToken);

        if (!response.Value.Flagged) return;

        throw new ModerationException("Content contains inappropriate material.");
    }

    /// <inheritdoc/>
    public async Task Moderate(string[] content, CancellationToken cancellationToken = default)
    {
        var response = await moderationClient.ClassifyTextAsync(content, cancellationToken);

        if (response.Value.Any(result => result.Flagged))
        {
            throw new ModerationException("Content contains inappropriate material.");
        }
    }
}