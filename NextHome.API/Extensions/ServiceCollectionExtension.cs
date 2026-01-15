using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NextHome.API.Constants;
using NextHome.Application.Extensions;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure;
using NextHome.Infrastructure.Extensions;
using NextHome.Infrastructure.Repositories;
using NextHome.QdrantService;
using NextHome.QdrantService.Extensions;
using OpenAI;
using NextHome.Application;
using NextHome.API.Options;
using NextHome.Application.Options;
using NextHome.Core.Interfaces.Repositories;
using NextHome.Infrastructure.Options;
using NextHome.QdrantService.Options;

namespace NextHome.API.Extensions;

/// <summary>
/// Service collection extension.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Configure services.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the CORS configuration will be added.</param>
    /// <param name="configuration">Builder configuration object.</param>
    /// <returns>The IServiceCollection with the CORS configuration applied.</returns>
    public static IServiceCollection ConfigureServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // Add options
        services.AddOptionsWithValidation<CorsOptions>(CorsOptions.SectionName, configuration);
        services.AddOptionsWithValidation<DatabaseOptions>(DatabaseOptions.SectionName, configuration);
        services.AddOptionsWithValidation<OpenAiOptions>(OpenAiOptions.SectionName, configuration);
        services.AddOptionsWithValidation<SwaggerOptions>(SwaggerOptions.SectionName, configuration);
        services.AddOptionsWithValidation<JwtOptions>(JwtOptions.SectionName, configuration);
        services.AddOptionsWithValidation<ApiOptions>(ApiOptions.SectionName, configuration);
        services.AddOptionsWithValidation<QdrantOptions>(QdrantOptions.SectionName, configuration);
        services.AddOptionsWithValidation<SmtpOptions>(SmtpOptions.SectionName, configuration);

        // Register OpenAI Client
        services.AddSingleton<OpenAIClient>(serviceProvider =>
        {
            var environmentOptions = serviceProvider.GetRequiredService<IOptions<OpenAiOptions>>().Value;
            return new OpenAIClient(environmentOptions.Key);
        });

        // Register all classes as scoped
        var currentAssembly = AssemblyReference.CurrentAssembly;
        var applicationAssembly = Application.AssemblyReference.CurrentAssembly;
        services.Scan(scan => scan
            .FromAssemblies(currentAssembly)
            .AddClasses()
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Add framework services.
        services.AddRouting();
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                applicationAssembly));

        // Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(SwaggerDocs.ApiVersion, new OpenApiInfo
            {
                Title = SwaggerDocs.ApiName,
                Version = SwaggerDocs.ApiVersion
            });

            options.CustomSchemaIds(description => description.FullName);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Provide JWT: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Infrastructure services
        var databaseOptions = GetOptions<DatabaseOptions>(configuration, DatabaseOptions.SectionName);
        services.AddInfrastructure(databaseOptions.Url);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExperienceCardRepository, ExperienceCardRepository>();
        services.AddScoped<IChallengeCardRepository, ChallengeCardRepository>();

        // Application services
        services.AddApplication();

        // Qdrant
        services.AddQdrant();

        // JWT
        var jwtOptions = GetOptions<JwtOptions>(configuration, JwtOptions.SectionName);
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Secret)
                    ),
                    ValidateLifetime = true
                };
            });
        
        return services;
    }

    /// <summary>
    /// Configures CORS by adding a CORS policy that allows requests from a specified client URL,
    /// including any headers and methods, and supports credentials.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the CORS configuration will be added.</param>
    /// <param name="configurationManager">Builder configuration object.</param>
    /// <returns>The IServiceCollection with the CORS configuration applied.</returns>
    public static IServiceCollection ConfigureCors(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        var envOptions = GetOptions<CorsOptions>(configurationManager, CorsOptions.SectionName);
        services.AddCors(options =>
        {
            options.AddPolicy(envOptions.PolicyName, policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(envOptions.ClientUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }

    /// <summary>
    /// Gets the environment options from configuration.
    /// </summary>
    /// <typeparam name="T">Generic type parameter representing the options type.</typeparam>
    /// <param name="configurationManager">Configuration manager instance.</param>
    /// <param name="sectionName">The name of the configuration section.</param>
    /// <returns>Options of type T from the specified configuration section.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the specified configuration section is not set.</exception>
    private static T GetOptions<T>(ConfigurationManager configurationManager, string sectionName)
    {
        return configurationManager.GetSection(sectionName).Get<T>() ??
                         throw new InvalidOperationException($"Configuration section '{sectionName}' not set.");
    }

    /// <summary>
    /// Adds options with validation to the service collection.
    /// </summary>
    /// <typeparam name="TOption">Options type.</typeparam>
    /// <param name="services">Service collection to which the options will be added.</param>
    /// <param name="sectionName">Section name in the configuration.</param>
    /// <param name="configuration">Configuration manager instance.</param>
    private static void AddOptionsWithValidation<TOption>(
        this IServiceCollection services,
        string sectionName,
        ConfigurationManager configuration)
        where TOption : class, new()
    {
        services.AddOptions<TOption>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}