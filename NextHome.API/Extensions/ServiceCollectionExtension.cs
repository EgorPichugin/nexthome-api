using System.Text;
using Microsoft.IdentityModel.Tokens;
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
    public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
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
        services.AddControllers();
        
        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                applicationAssembly));

        // TODO: add auth for swagger
        // Swagger
        services.AddSwaggerGen();
        
        // Db context
        services.AddInfrastructure(configuration);
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        // JWT
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            });
        


        return services;
    }

    /// <summary>
    /// Configures CORS by adding a CORS policy that allows requests from a specified client URL,
    /// including any headers and methods, and supports credentials.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the CORS configuration will be added.</param>
    /// <returns>The IServiceCollection with the CORS configuration applied.</returns>
    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(EnvironmentManager.CorsPolicyName, policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(EnvironmentManager.ClientUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}