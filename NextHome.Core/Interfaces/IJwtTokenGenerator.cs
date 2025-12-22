namespace NextHome.Core.Interfaces;

/// <summary>
/// JWT token generator.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generates a JSON Web Token (JWT) for a specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the token is being generated.</param>
    /// <param name="email">The email address of the user for whom the token is being generated.</param>
    /// <returns>A string representing the generated JWT token.</returns>
    string GenerateToken(Guid userId, string email);
}