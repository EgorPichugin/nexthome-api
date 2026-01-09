using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NextHome.Application.Common.Validation;
using NextHome.Application.Countries.Services;
using NextHome.QdrantService;
using OpenAI;
using OpenAI.Embeddings;
using Qdrant.Client;

namespace NextHome.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<ICardValidationService, CardValidationService>();

        var qdrantOptions = services.BuildServiceProvider().GetService<IOptions<QdrantOptions>>();
        if (qdrantOptions == null)
        {
            throw new InvalidOperationException("Qdrant options not set.");
        }

        services.AddSingleton<QdrantClient>(_ =>
            new QdrantClient(qdrantOptions.Value.Host, qdrantOptions.Value.Port));
        services.AddSingleton<OpenAIClient>(_ => new OpenAIClient(qdrantOptions.Value.OpenAiKey));

        services.AddSingleton<EmbeddingClient>(serviceProvider =>
        {
            var openAi = serviceProvider.GetRequiredService<OpenAIClient>();
            return openAi.GetEmbeddingClient(Constants.EmbeddingModel);
        });

        return services;
    }
}