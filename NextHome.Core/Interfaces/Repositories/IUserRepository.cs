using NextHome.Core.Entities;

namespace NextHome.Core.Interfaces.Repositories;

/// <summary>
/// Contract for a user repository.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="userEntity">The user entity to be added.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the created user entity.</returns>
    Task<UserEntity> Add(UserEntity userEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user entity by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user entity if found, or null if no user exists with the provided email address.</returns>
    Task<UserEntity?> GetByEmail(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all user entities from the repository.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of all user entities in the repository.</returns>
    Task<List<UserEntity>> GetAll(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a user with the specified email address exists in the repository.
    /// </summary>
    /// <param name="email">The email address to check for existence.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing a boolean value indicating whether a user with the specified email address exists.</returns>
    Task<bool> Exists(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user entity by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user entity if found, or null if no user exists with the provided identifier.</returns>
    Task<UserEntity?> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    /// <param name="userEntity">The user entity with updated properties.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Update(UserEntity userEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing user.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task Delete(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user entity by their authentication ID.
    /// </summary>
    /// <param name="authId">The authentication ID of the user to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the user entity if found, or null if no user exists with the provided authentication ID.</returns>
    Task<UserEntity?> GetByAuthId(string authId, CancellationToken cancellationToken = default);
}