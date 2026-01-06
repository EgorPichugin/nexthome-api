using Microsoft.Extensions.DependencyInjection;

namespace NextHome.QdrantService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddQdrant(
        this IServiceCollection services)
    {
        services.AddScoped<IQdrantService, QdrantService>();

        return services;
    }
}