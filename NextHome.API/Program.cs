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
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.ConfigureServices(builder.Configuration);
        builder.Services.ConfigureCors(builder.Configuration);

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            appDbContext.Database.Migrate();
        }

        app.ConfigureApp();
        app.Run();
    }
}