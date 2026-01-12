using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.Configuration;
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
        services.AddOptions<QdrantOptions>()
            .Bind(configuration.GetSection(QdrantOptions.Qdrant))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<EnvironmentOptions>()
            .Bind(configuration.GetSection(EnvironmentOptions.Environment))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<OpenAiOptions>()
            .Bind(configuration.GetSection(OpenAiOptions.OpenAi))
            .ValidateDataAnnotations()
            .ValidateOnStart();

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
        var envOptions = GetEnvironmentOptions(configuration);
        services.AddInfrastructure(configuration, envOptions.DatabaseUrl);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExperienceCardRepository, ExperienceCardRepository>();
        services.AddScoped<IChallengeCardRepository, ChallengeCardRepository>();

        // Application services
        services.AddApplication();

        // Qdrant
        services.AddQdrant();

        // JWT
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
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
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
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
        var envOptions = GetEnvironmentOptions(configurationManager);
        services.AddCors(options =>
        {
            options.AddPolicy(envOptions.CorsPolicyName, policyBuilder =>
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
    /// Retrieves values for environment options.
    /// </summary>
    /// <param name="configurationManager">Build configuration object.</param>
    /// <returns>The <see cref="EnvironmentOptions"/> instance.</returns>
    private static EnvironmentOptions GetEnvironmentOptions(ConfigurationManager configurationManager)
    {
        return configurationManager.GetSection(EnvironmentOptions.Environment).Get<EnvironmentOptions>() ??
                         throw new InvalidOperationException("Environment options not set.");
    }
}