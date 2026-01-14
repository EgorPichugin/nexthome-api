namespace NextHome.Core.Interfaces;

/// <summary>
/// Interface for generating and hashing tokens.
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// Generates a secure token.
    /// </summary>
    /// <param name="length">The length of the token to generate. The default value is 32.</param>
    /// <returns>A secure token as a string.</returns>
    string GenerateToken(int length = 32);

    /// <summary>
    /// Hashes the given token.
    /// </summary>
    /// <param name="token">The token to hash.</param>
    /// <returns>The hashed token as a string.</returns>
    string HashToken(string token);

    /// <summary>
    /// Generates a JSON Web Token (JWT) for a specified user.
    /// </summary>
    /// <param name="userId">The ID of the user for whom the token is being generated.</param>
    /// <param name="email">The email of the user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    string GenerateJwtToken(Guid userId, string email);
}