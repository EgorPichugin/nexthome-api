using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NextHome.Core.Interfaces;
using NextHome.QdrantService.Options;
using OpenAI;
using OpenAI.Embeddings;
using Qdrant.Client;

namespace NextHome.QdrantService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddQdrant(
        this IServiceCollection services)
    {
        services.AddSingleton<IQdrantService, QdrantService>();
        
        // Register QdrantClient
        services.AddSingleton<QdrantClient>(serviceProvider =>
        {
            var qdrantOptions = serviceProvider.GetRequiredService<IOptions<QdrantOptions>>().Value;
            return new QdrantClient(qdrantOptions.Host, qdrantOptions.Port);
        });

        // Register OpenAi EmbeddingClient
        services.AddSingleton<EmbeddingClient>(serviceProvider =>
        {
            var openAiClient = serviceProvider.GetRequiredService<OpenAIClient>();
            return openAiClient.GetEmbeddingClient(Constants.EmbeddingModel);
        });

        return services;
    }
}