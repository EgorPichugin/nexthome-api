namespace NextHome.Application.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when content is rejected by moderation.
/// </summary>
public sealed class ModerationException : Exception
{
    public ModerationException(string message) : base(message) {}

    public ModerationException(string message, Exception innerException) : base(message, innerException) {}
}
