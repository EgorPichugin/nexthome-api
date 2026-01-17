using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NextHome.Application.Common.Validation;
using NextHome.Application.Countries.Services;
using NextHome.Application.Options;
using NextHome.Application.Users.Services;
using NextHome.Core.Interfaces.Services;
using NextHome.Infrastructure.Services;
using OpenAI;
using OpenAI.Moderations;

namespace NextHome.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IUserValidationService, UserValidationService>();
        services.AddScoped<ICardValidationService, CardValidationService>();
        services.AddScoped<IModerationService, ModerationService>();
        services.AddSingleton<IUserEntityMapper, UserEntityMapper>();

        // Register OpenAI ModerationClient
        services.AddSingleton<ModerationClient>(serviceProvider =>
        {
            var openAiClient = serviceProvider.GetRequiredService<OpenAIClient>();
            return openAiClient.GetModerationClient(Constants.ChatModel);
        });

        return services;
    }
}