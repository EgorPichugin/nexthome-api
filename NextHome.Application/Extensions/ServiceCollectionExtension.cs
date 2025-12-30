using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextHome.Application.Countries.Services;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICsvReaderService, CsvReaderService>();

        return services;
    }
}