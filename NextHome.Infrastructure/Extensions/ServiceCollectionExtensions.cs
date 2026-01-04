using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NextHome.Infrastructure.Persistence;

namespace NextHome.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string databaseUrl)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(GetConnectionString(databaseUrl)));

        return services;
    }

    // TODO: fix it later
    private static string GetConnectionString(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');

        return $"Host={uri.Host};" +
               $"Port={uri.Port};" +
               $"Database={uri.AbsolutePath.Trim('/')};" +
               $"Username={userInfo[0]};" +
               $"Password={userInfo[1]}";
    }
}