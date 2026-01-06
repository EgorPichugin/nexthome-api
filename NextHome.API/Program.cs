using Microsoft.EntityFrameworkCore;
using NextHome.API.Extensions;
using NextHome.Infrastructure.Persistence;
using NextHome.QdrantService;

namespace NextHome.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions<QdrantOptions>()
            .Bind(builder.Configuration.GetSection(QdrantOptions.Qdrant))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var envOptions = builder.Configuration.GetSection(EnvironmentOptions.Environment)
                             .Get<EnvironmentOptions>()
                         ?? throw new InvalidOperationException("Environment options not set.");

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
}