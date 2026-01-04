using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NextHome.API.Constants;
using NextHome.Application.Extensions;
using NextHome.Core.Interfaces;
using NextHome.Infrastructure;
using NextHome.Infrastructure.Extensions;
using NextHome.Infrastructure.Repositories;

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
    public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration, EnvironmentOptions envOptions)
    {
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
        services.AddInfrastructure(configuration, envOptions.DATABASE_URL);
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExperienceCardRepository, ExperienceCardRepository>();
        services.AddScoped<IChallengeCardRepository, ChallengeCardRepository>();
        
        // Application services
        services.AddApplication();
        
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
    /// <param name="envOptions">Environment options that allow to retrieve set variables.</param>
    /// <returns>The IServiceCollection with the CORS configuration applied.</returns>
    public static IServiceCollection ConfigureCors(this IServiceCollection services, EnvironmentOptions envOptions)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(envOptions.CORS_POLICY_NAME, policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(envOptions.CLIENT_URL)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}