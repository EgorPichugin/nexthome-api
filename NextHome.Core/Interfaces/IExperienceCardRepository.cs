using NextHome.Core.Entities;

namespace NextHome.Core.Interfaces;

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
}