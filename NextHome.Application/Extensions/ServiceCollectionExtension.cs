using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextHome.Application.Common.Validation;
using NextHome.Application.Countries.Services;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<IExperienceCardValidationService, ExperienceCardValidationService>();

        return services;
    }
}