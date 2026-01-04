using Microsoft.EntityFrameworkCore;
using NextHome.API.Extensions;
using NextHome.Infrastructure.Persistence;

namespace NextHome.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var envOptions = getEnvironmentOptions(builder);
        
        builder.Services.ConfigureServices(builder.Configuration, envOptions);
        builder.Services.ConfigureCors(envOptions);
        
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.Migrate();
        }

        
        app.ConfigureApp(envOptions);
        app.Run();
    }

    private static EnvironmentOptions getEnvironmentOptions(WebApplicationBuilder builder)
    {
        var envOptions = builder.Configuration.Get<EnvironmentOptions>()
                         ?? throw new InvalidOperationException("EnvironmentOptions not found");

        if (string.IsNullOrWhiteSpace(envOptions.CORS_POLICY_NAME))
            throw new InvalidOperationException("Cors policy name not set.");
        
        if (string.IsNullOrWhiteSpace(envOptions.CLIENT_URL))
            throw new InvalidOperationException("Client URL not set.");
        
        if (string.IsNullOrWhiteSpace(envOptions.DATABASE_URL))
            throw new InvalidOperationException("Database URL not set.");
        
        return envOptions;
    }
}
