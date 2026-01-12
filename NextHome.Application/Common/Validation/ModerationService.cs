using NextHome.Application.Common.Exceptions;
using OpenAI.Moderations;

namespace NextHome.Application.Common.Validation;

/// <summary>
/// Service for content moderation using OpenAI ModerationClient.
/// </summary>
public interface IModerationService
{
    /// <summary>
    /// Moderates the given content.
    /// </summary>
    /// <param name="content">The content to be moderated.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Moderate(string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Moderates the given array of content.
    /// </summary>
    /// <param name="content">The array of content to be moderated.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Moderate(string[] content, CancellationToken cancellationToken = default);
}

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

        foreach (var result in response.Value)
        {
            if (result.Flagged)
            {
                throw new ModerationException("Content contains inappropriate material.");
            }
        }
    }
}