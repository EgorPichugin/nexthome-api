using NextHome.Core.Entities;

namespace NextHome.Core.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing challenge card entities.
/// </summary>
public interface IChallengeCardRepository
{
    /// <summary>
    /// Adds a new challenge card to the repository.
    /// </summary>
    /// <param name="challengeCardEntity">The challenge card entity to add.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The added challenge card entity.</returns>
    Task<ChallengeCardEntity> Add(ChallengeCardEntity challengeCardEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all challenge cards associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A list of challenge cards belonging to the user.</returns>
    Task<List<ChallengeCardEntity>> GetChallengeCardsByUserId(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a challenge card by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the challenge card.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The challenge card entity if found; otherwise, null.</returns>
    Task<ChallengeCardEntity?> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a challenge card by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the challenge card to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    Task Delete(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing challenge card in the repository.
    /// </summary>
    /// <param name="challengeCardEntity">The challenge card entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    Task Update(ChallengeCardEntity challengeCardEntity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns list of <see cref="ExperienceCardEntity"/> by list of Ids.
    /// </summary>
    /// <param name="ids">List of ids which cards should be returned.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>List of <see cref="ExperienceCardEntity"/>.</returns>
    Task<List<ExperienceCardEntity>> GetExperienceCardsByIds(List<Guid> ids, CancellationToken cancellationToken = default);
}