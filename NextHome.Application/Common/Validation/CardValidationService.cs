using NextHome.Application.Users.Interfaces;

namespace NextHome.Application.Common.Validation;

/// <summary>
/// Defines a service for validating experience card information provided in user-related operations.
/// </summary>
public interface ICardValidationService
{
    /// <summary>
    /// Validates the provided card information for a specific user, ensuring that the user and card exist
    /// and the input data conforms to validation rules.
    /// </summary>
    /// <param name="request">The request containing the card request data.</param>
    /// <returns>A list of validation error messages if the validation fails; an empty list if the validation is successful.</returns>
    List<string> Validate(ICardRequest request);
}

/// <inheritdoc />
public class CardValidationService : ICardValidationService
{
    /// <inheritdoc/>
    public List<string> Validate(ICardRequest request)
    {
        var errors = new List<string>();
        
        if (!IsValidTitle(request.Title)) errors.Add("Title is required.");
        if (!IsValidDescription(request.Description, 50)) errors.Add("Description is empty or too short. It should be 50 symbols at least.");
        
        return errors;
    }

    /// <summary>
    /// Determines whether the given title is valid by checking if it is not null, empty, or whitespace.
    /// </summary>
    /// <param name="name">The title of the experience card to validate.</param>
    /// <returns>True if the title is valid; otherwise, false.</returns>
    private static bool IsValidTitle(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    /// <summary>
    /// Validates the description field to ensure it is not null, empty, or whitespace.
    /// </summary>
    /// <param name="description">The description to validate.</param>
    /// <param name="length">The minimum length for a description of the experience.</param>
    /// <returns>A boolean value indicating whether the description is valid.</returns>
    private static bool IsValidDescription(string description, int length)
    {
        return !string.IsNullOrWhiteSpace(description) &&  description.Length > length;
    }
}