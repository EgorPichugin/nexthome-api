using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NextHome.Core.Entities;
using Qdrant.Client.Grpc;
using Qdrant.Client;
using OpenAI;
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
    Task<UpdateResult> StoreExperienceCard(ExperienceCardEntity card, string country,
        string collectionName = Constants.DefaultCollectionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for similar cards in Qdrant.
    /// </summary>
    /// <param name="card"><see cref="ChallengeCardEntity"/>.</param>
    /// <param name="country">The country of the user.</param>
    /// <param name="collectionName">Collection name where to search.</param>
    /// <param name="limit">Limit for nmber of found cards.</param>
    /// <param name="scoreThreshold">Score threshold to avoid cards skip cards below it.</param>
    /// <param name="cancellationToken">Token to cancel the transactions.</param>
    /// <returns>List of similar found cards.</returns>
    Task<List<ScoredPoint>> SearchSimilarCards(ChallengeCardEntity card, string country,
        string collectionName = Constants.DefaultCollectionName, ulong limit = 3, float scoreThreshold = 0.4f,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all collection names.
    /// </summary>
    /// <returns>List of Qdrant collection names.</returns>
    Task<List<string>> GetCollectionList();
}

/// <summary>
/// Services qdrant requests.
/// </summary>
public sealed class QdrantService : IQdrantService
{
    /// <summary>
    /// Default collection name for experience cards.
    /// </summary>
    private const string CountryFilter = "country";

    /// <summary>
    /// Client to connect to qdrant instance.
    /// </summary>
    private readonly QdrantClient _client;

    /// <summary>
    /// Embedding the client that is responsible for embeddings.
    /// </summary>
    private readonly EmbeddingClient _embeddingClient;

    /// <summary>
    /// Logger for qdrant service.
    /// </summary>
    private readonly ILogger<QdrantService> _logger;

    public QdrantService(IOptions<QdrantOptions> options, ILogger<QdrantService> logger)
    {
        _logger = logger;
        // TODO: delete after testing
        _logger.LogInformation("QDRANT OPTIONS: {host}, {port}, {key}", options.Value.Host, options.Value.Port, options.Value.OpenAiKey);
        _client = new QdrantClient(options.Value.Host, options.Value.Port);
        var openAiClient = new OpenAIClient(options.Value.OpenAiKey);
        _embeddingClient = openAiClient.GetEmbeddingClient(Constants.EmbeddingModel);
    }

    /// <inheritdoc />
    public async Task CreateCollection(string? collectionName = Constants.DefaultCollectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var collections = await _client.ListCollectionsAsync(cancellationToken);
        if (collections.Contains(collectionName)) return;

        var vectorParams = new VectorParams
        {
            Size = 1536,
            Distance = Distance.Cosine
        };

        await _client.CreateCollectionAsync(
            collectionName,
            vectorParams,
            cancellationToken: cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task DeleteCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        await _client.DeleteCollectionAsync(collectionName: collectionName, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<UpdateResult> StoreExperienceCard(
        ExperienceCardEntity card,
        string country,
        string? collectionName = Constants.DefaultCollectionName,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var embeddings = await GetEmbeddings(card, cancellationToken);
        var point = new PointStruct()
        {
            Id = card.Id,
            Vectors = embeddings,
            Payload = { [CountryFilter] = country }
        };

        try
        {
            var result = await _client.UpsertAsync(
                collectionName: collectionName,
                points: [point],
                wait: true,
                cancellationToken: cancellationToken);

            _logger.LogInformation("🟩 [QDRANT] upsert OK card={CardId}", card.Id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "🟥 [QDRANT] upsert FAILED card={CardId}", card.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<ScoredPoint>> SearchSimilarCards(ChallengeCardEntity card, string country,
        string? collectionName = Constants.DefaultCollectionName, ulong limit = 3, float scoreThreshold = 0.4f,
        CancellationToken cancellationToken = default)
    {
        collectionName ??= Constants.DefaultCollectionName;
        var embeddings = await GetEmbeddings(card, cancellationToken);

        try
        {
            var searchResult = await _client.QueryAsync(
                collectionName: collectionName,
                query: embeddings,
                limit: limit,
                filter: MatchKeyword(CountryFilter, country),
                payloadSelector: true,
                scoreThreshold: scoreThreshold,
                cancellationToken: cancellationToken);
            _logger.LogInformation("🟥 [OPENAI] similar search OK card={CardId}", card.Id);
            return searchResult.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "🟥 [OPENAI] similar search FAILED");
            throw;     
        }
    }

    /// <inheritdoc />
    public async Task<List<string>> GetCollectionList()
    {
        try
        {
            _logger.LogInformation("[QDRANT] get collection list started");
            
            _logger.LogInformation("[QDRANT] get collection list started");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[QDRANT] get collection list FAILED");
        }
        var response = await _client.ListCollectionsAsync();
        return response.ToList();
    }

    /// <summary>
    /// Generate embeddings for card description.
    /// </summary>
    /// <param name="card"><see cref="ICardEntity"/>.</param>
    /// <param name="cancellationToken">Token to cancel the transactions.</param>
    /// <returns>Vectorized form of card description.</returns>
    private async Task<float[]> GetEmbeddings(ICardEntity card, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("[OPENAI] embeddings start");

            var response = await _embeddingClient.GenerateEmbeddingAsync(
                card.Description.ToLower(), cancellationToken: cancellationToken);

            var vector = response.Value.ToFloats().ToArray();

            if (vector == null || vector.Length == 0)
            {
                throw new InvalidOperationException("Embedding vector null or empty.");
            }

            _logger.LogInformation("[OPENAI] embeddings OK size={Size}", vector.Length);

            return vector;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OPENAI] embeddings FAILED");
            throw;
        }
    }
}