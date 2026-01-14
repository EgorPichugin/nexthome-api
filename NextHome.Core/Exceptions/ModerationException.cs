namespace NextHome.Core.Exceptions;

/// <summary>
/// Represents an exception thrown when moderation rejects content.
/// </summary>
public sealed class ModerationException : Exception
{
    public ModerationException(string message) : base(message) {}

    public ModerationException(string message, Exception innerException) : base(message, innerException) {}
}
