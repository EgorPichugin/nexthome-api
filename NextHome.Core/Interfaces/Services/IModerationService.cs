namespace NextHome.Core.Interfaces.Services;

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