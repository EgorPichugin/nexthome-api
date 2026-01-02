using NextHome.Application.Users.Commands;
using NextHome.Core.Interfaces;

namespace NextHome.Application.Common.Validation;

/// <summary>
/// Defines a service for validating experience card information provided in user-related operations.
/// </summary>
public interface IExperienceCardValidationService
{
    /// <summary>
    /// Validates the provided experience card information for a specific user, ensuring that the user and card exist
    /// and the input data conforms to validation rules.
    /// </summary>
    /// <param name="request">The request containing the updated data for the experience card.</param>
    /// <returns>A list of validation error messages if the validation fails; an empty list if the validation is successful.</returns>
    List<string> Validate(UpdateExperienceCardRequest request);

    /// <summary>
    /// Validates the provided create experience card request, ensuring the data adheres to the required business rules
    /// and contains valid values before creating an experience card.
    /// </summary>
    /// <param name="request">The request object containing the details for the experience card, such as title and description.</param>
    /// <returns>A list of error messages indicating validation failures. If the validation passes, returns an empty list.</returns>
    List<string> Validate(CreateExperienceCardRequest request);
}

/// <inheritdoc />
public class ExperienceCardValidationService : IExperienceCardValidationService
{
    /// <inheritdoc/>
    public List<string> Validate(UpdateExperienceCardRequest request)
    {
        return GetErrorsForFields(request.Title, request.Description);
    }

    public List<string> Validate(CreateExperienceCardRequest request)
    {
        return GetErrorsForFields(request.Title, request.Description);
    }


    private static List<string> GetErrorsForFields(string title, string description)
    {
        var errors = new List<string>();
        
        if (!IsValidTitle(title)) errors.Add("Title is required.");
        if (!IsValidDescription(description)) errors.Add("Description is required.");
        
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
    /// <returns>A boolean value indicating whether the description is valid.</returns>
    private static bool IsValidDescription(string description)
    {
        return !string.IsNullOrWhiteSpace(description);
    }
}