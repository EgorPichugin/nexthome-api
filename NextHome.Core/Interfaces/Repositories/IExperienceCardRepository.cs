using NextHome.Core.Entities;

namespace NextHome.Core.Interfaces.Repositories;

/// <summary>
/// Defines the repository responsible for managing experience card operations.
/// </summary>
public interface IExperienceCardRepository
{
    /// <summary>
    /// Adds a new experience card to the repository.
    /// </summary>
    /// <param name="experienceCardEntity">
    /// The experience card entity to add to the repository. This includes details such as Title, Description, UserId, and other relevant properties.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the added experience card entity, including any changes or additional details
    /// set during the insertion process.
    /// </returns>
    Task<ExperienceCardEntity> Add(ExperienceCardEntity experienceCardEntity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of experience cards associated with a specific user.
    /// </summary>
    /// <param name="userId">
    /// The unique identifier of the user whose experience cards are to be retrieved.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of
    /// experience card entities belonging to the specified user.
    /// </returns>
    Task<List<ExperienceCardEntity>> GetExperienceCardsByUserId(Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an experience card from the repository based on the specified identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the experience card to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the experience card entity if found; otherwise, null.
    /// </returns>
    Task<ExperienceCardEntity?> GetById(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing experience card in the repository with new details.
    /// </summary>
    /// <param name="experienceCardEntity">
    /// The experience card entity containing updated information. This includes properties such as Title, Description, and associated UserId.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the updated experience card entity with the applied changes.
    /// </returns>
    Task Update(ExperienceCardEntity experienceCardEntity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an experience card from the repository based on the provided identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the experience card to be deleted.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task completes once the experience card
    /// is successfully deleted from the repository.
    /// </returns>
    Task Delete(Guid id, CancellationToken cancellationToken = default);
}