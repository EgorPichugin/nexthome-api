using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NextHome.Core.Entities;
using Qdrant.Client.Grpc;
using Qdrant.Client;

namespace NextHome.QdrantService;

public interface IQdrantService
{
    // public QdrantOptions Options { get; }
}

public sealed class QdrantService : IQdrantService
{
    private readonly QdrantOptions _options;
    private readonly QdrantClient _client;

    public QdrantService(IOptions<QdrantOptions> options)
    {
        _options = options.Value;
        _client = new QdrantClient(_options.Host, _options.Port);
    }

    public async Task CreateCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        var vectorParams = new VectorParams
        {
            Size = 4,
            Distance = Distance.Dot
        };
        await _client.CreateCollectionAsync(collectionName: collectionName, vectorsConfig: vectorParams,
            cancellationToken: cancellationToken);
    }

    public async Task DeleteCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        await _client.DeleteCollectionAsync(collectionName: collectionName, cancellationToken: cancellationToken);
    }

    public async Task StoreExperienceCard(ExperienceCardEntity experienceCardEntity,
        CancellationToken cancellationToken = default)
    {
        
    }
}