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

        return services;
    }
}