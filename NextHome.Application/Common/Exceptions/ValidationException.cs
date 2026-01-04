namespace NextHome.Application.Common.Exceptions;

/// <summary>
/// Represents an exception that occurs when validation fails.
/// </summary>
/// <param name="errors">The list of validation errors.</param>
public class ValidationException(IEnumerable<string> errors) : Exception("Validation failed")
{
    public IReadOnlyList<string> Errors { get; } = errors.ToList();
}
