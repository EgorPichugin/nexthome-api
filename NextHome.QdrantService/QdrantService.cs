using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NextHome.Core.Entities;
using Qdrant.Client.Grpc;
using Qdrant.Client;
using OpenAI.Embeddings;
using static Qdrant.Client.Grpc.Conditions;

namespace NextHome.QdrantService;

/// <summary>
/// Contract for qdrant service.
/// </summary>
public interface IQdrantService
{
    /// <summary>
    /// Creates new collection in Qdrant.
    /// </summary>
    /// <param name="collectionName">Collection name. Has a default name.</param>
    /// <param name="cancellationToken">Token to cancel the transaction.</param>
    Task CreateCollection(string? collectionName = Constants.DefaultCollectionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes collection in Qdrant.
    /// </summary>
    /// <param name="collectionName">Collection name that should be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the transaction.</param>
    Task DeleteCollection(string collectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores experience card in Qdrant. 
    /// </summary>
    /// <param name="card"><see cref="ExperienceCardEntity"/>.</param>
    /// <param name="country">The country of the user.</param>
    /// <param name="collectionName">Collection name where to put the card.</param>
    /// <param name="cancellationToken">Token to cancel the transaction.</param>
    /// <returns>Object that contains common information about stored card.</returns>
    Task<UpdateResult> SaveExperienceCard(ExperienceCardEntity card, string country,
        string collectionName = Constants.DefaultCollectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for similar cards in Qdrant.
    /// </summary>
    /// <param name="card"><see cref="ChallengeCardEntity"/>.</param>
    /// <param name="country">The country of the user.</param>
    /// <param name="collectionName">Collection name where to search.</param>
    /// <param name="limit">Limit for number of found cards.</param>
    /// <param name="scoreThreshold">Score threshold to avoid cards skip cards below it.</param>
    /// <param name="cancellationToken">Token to cancel the transactions.</param>
    /// <returns>List of similar found cards.</returns>
    Task<List<ScoredPoint>> SearchSimilarCards(ChallengeCardEntity card, string country,
        string collectionName = Constants.DefaultCollectionName, ulong limit = 3, float scoreThreshold = 0.4f,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all collection names.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the transactions.</param>
    /// <returns>List of Qdrant collection names.</returns>
    Task<List<string>> GetCollectionList(CancellationToken cancellationToken);

    /// <summary>
    /// Deletes experience card from Qdrant.
    /// </summary>
    /// <param name="card"><see cref="ExperienceCardEntity"/> that should be deleted.</param>
    /// <param name="collectionName">Collection where the card is stored.</param>
    /// <param name="cancellationToken">Token to cancel the transaction.</param>
    Task DeleteExperienceCard(ExperienceCardEntity card, string? collectionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes experience cards from Qdrant.
    /// </summary>
    /// <param name="cards">Array of <see cref="ExperienceCardEntity"/> that should be deleted.</param>
    /// <param name="collectionName">Collection where the cards are stored.</param>
    /// <param name="cancellationToken">Token to cancel the transaction.</param>
    /// <returns>A task representing the asynchronous operation. The task completes once all specified experience cards are successfully deleted from Qdrant.`</returns>
    Task DeleteExperienceCards(IReadOnlyCollection<ExperienceCardEntity> cards, string? collectionName,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Services qdrant requests.
/// </summary>
public sealed class QdrantService(
    QdrantClient client,
    EmbeddingClient embeddingClient,
    ILogger<QdrantService> logger)
    : IQdrantService
{
    /// <summary>
    /// Default collection name for experience cards.
    /// </summary>
    private const string CountryFilter = "country";

    /// <summary>
    /// Default size of vector for Qdrant.
    /// </summary>
    private const int Size = 1536;

    /// <inheritdoc />
    public async Task CreateCollection(string? collectionName = Constants.DefaultCollectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var collections = await GetCollectionList(cancellationToken);
        if (collections.Contains(collectionName)) return;

        var vectorParams = new VectorParams
        {
            Size = Size,
            Distance = Distance.Cosine
        };

        await ExecuteAsync(
            () => client.CreateCollectionAsync(
                collectionName, vectorParams, cancellationToken: cancellationToken),
                    "{Field} Creating collection started",
            "{Field} Collection with name {collectionName} successfully created",
            "{Field} Collection name creating failed",
            ["[QDRANT]:", collectionName]);
    }

    /// <inheritdoc />
    public async Task DeleteCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        await ExecuteAsync(
            () => client.DeleteCollectionAsync(collectionName: collectionName, cancellationToken: cancellationToken),
            "{Field} Deleting collection started",
            "{Field} Collection with name {collectionName} successfully deleted",
            "{Field} Deleting collection name failed.",
            ["[QDRANT]:", collectionName]);
    }

    /// <inheritdoc />
    public async Task<UpdateResult> SaveExperienceCard(
        ExperienceCardEntity card,
        string country,
        string? collectionName = Constants.DefaultCollectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        
        var embeddings = await ExecuteAsync(
            () => GetEmbeddings(card, cancellationToken),
            "{Field} Embeddings started",
            "{Field} Embeddings OK size={Size}",
            "{Field} embeddings failed", 
            ["[OPENAI]:", Size]);

        var point = new PointStruct()
        {
            Id = card.Id,
            Vectors = embeddings,
            Payload = { [CountryFilter] = country }
        };

        var result = await ExecuteAsync(
            () => client.UpsertAsync(
                collectionName: collectionName,
                points: [point],
                wait: true,
                cancellationToken: cancellationToken),
            "{Field} Upserting experience card started",
            "{Field} Upsert OK card={CardId}", 
            "{Field} Upsert FAILED card={CardId}", 
            ["[QDRANT]:", card.Id]);

        return result;
    }

    /// <inheritdoc />
    public async Task<List<ScoredPoint>> SearchSimilarCards(ChallengeCardEntity card, string country,
        string? collectionName = Constants.DefaultCollectionName, ulong limit = 3, float scoreThreshold = 0.4f,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var embeddings = await ExecuteAsync(
            () => GetEmbeddings(card, cancellationToken),
            "{Field} Embeddings started",
            "{Field} Embeddings OK size={Size}", 
            "{Field} Embeddings failed", 
            ["[OPENAI]:", Size]);
        
        var searchResult = await ExecuteAsync(
            () => client.QueryAsync(
                collectionName: collectionName,
                query: embeddings,
                limit: limit,
                filter: MatchKeyword(CountryFilter, country),
                payloadSelector: true,
                scoreThreshold: scoreThreshold,
                cancellationToken: cancellationToken),
            "{Field} Search of similar cards started",
            "{Field} Similar search OK card={CardId}", 
            "{Field} Similar search failed",
            ["[QDRANT]:", card.Id]);

        return searchResult.ToList();
    }

    /// <inheritdoc />
    public async Task<List<string>> GetCollectionList(CancellationToken cancellationToken)
    {
        var collections = await ExecuteAsync(
            () => client.ListCollectionsAsync(cancellationToken),
            "{Field} Retrieving collection names started",
            "{Field} Collection names retrieved from Qdrant",
            "{Field} Retrieving collection names failed",
            "[QDRANT]:");

        return collections.ToList();
    }

    /// <inheritdoc />
    public async Task DeleteExperienceCard(ExperienceCardEntity card, string? collectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        await ExecuteAsync(
            () => client.DeleteAsync(
                collectionName: collectionName,
                id: card.Id,
                cancellationToken: cancellationToken),
            "{Field} Deleting experience card started",
            "{Field} Experience card {CardId} successfully deleted",
            "{Field} Deleting experience card failed",
            ["[QDRANT]:", card.Id]);
    }
    
    /// <inheritdoc />
    public async Task DeleteExperienceCards(IReadOnlyCollection<ExperienceCardEntity> cards, string? collectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var ids = cards.Select(card => card.Id).ToList();
        await ExecuteAsync(
            () => client.DeleteAsync(
                collectionName: collectionName,
                ids: ids,
                cancellationToken: cancellationToken),
            "{Field} Deleting  experience cards started",
            "{Field} Deleting experience cards successfully deleted",
            "{Field} Deleting experience cards failed",
            ["[QDRANT]:", ids]);
    }

    /// <summary>
    /// Generate embeddings for card description.
    /// </summary>
    /// <param name="card"><see cref="ICardEntity"/>.</param>
    /// <param name="cancellationToken">Token to cancel the transactions.</param>
    /// <returns>Vectorized form of card description.</returns>
    private async Task<float[]> GetEmbeddings(ICardEntity card, CancellationToken cancellationToken)
    {
        var response = await embeddingClient.GenerateEmbeddingAsync(
            card.Description.ToLower(), cancellationToken: cancellationToken);

        var vector = response.Value.ToFloats().ToArray() ??
                     throw new InvalidOperationException("Embedding vector null or empty.");

        return vector;
    }

    /// <summary>
    /// Executes an asynchronous action with logging for start, success, and error events.
    /// </summary>
    /// <typeparam name="T">Type of the result returned by the asynchronous action.</typeparam>
    /// <param name="action">An asynchronous action to execute.</param>
    /// <param name="startingLog">Log message to indicate the start of the action.</param>
    /// <param name="successLog">Log message to indicate the successful completion of the action.</param>
    /// <param name="errorLog">Log message to indicate an error occurred during the action.</param>
    /// <param name="logArgs">Arguments to be included in the log messages.</param>
    /// <returns>The result of the asynchronous action.</returns>
    private async Task<T> ExecuteAsync<T>(
        Func<Task<T>> action,
        string startingLog,
        string successLog,
        string errorLog,
        params object[] logArgs)
    {
        try
        {
            logger.LogInformation(startingLog, logArgs);
            var result = await action();
            logger.LogInformation(successLog, logArgs);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, errorLog, logArgs);
            throw;
        }
    }

    /// <summary>
    /// Executes an asynchronous action with logging for start, success, and error events.
    /// </summary>
    /// <param name="action">An asynchronous action to execute.</param>
    /// <param name="startingLog">Log message to indicate the start of the action.</param>
    /// <param name="successLog">Log message to indicate the successful completion of the action.</param>
    /// <param name="errorLog">Log message to indicate an error occurred during the action.</param>
    /// <param name="logArgs">Arguments to be included in the log messages.</param>
    /// <returns></returns>
    private async Task ExecuteAsync(
        Func<Task> action,
        string startingLog,
        string successLog,
        string errorLog,
        params object[] logArgs)
    {
        try
        {
            logger.LogInformation(startingLog, logArgs);
            await action();
            logger.LogInformation(successLog, logArgs);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, errorLog, logArgs);
            throw;
        }
    }
}