namespace NextHome.Application.Countries.Responses;

/// <summary>
/// Represents a response containing information about a country.
/// </summary>
/// <param name="Name">The name of the country.</param>
/// <param name="Code">The code of the country.</param>
public record CountryResponse(
    string Name,
    string Code);