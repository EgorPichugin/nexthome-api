using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NextHome.Core.Interfaces.Services;
using NextHome.Infrastructure.Persistence;
using NextHome.Infrastructure.Services;

namespace NextHome.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseUrl)
    {
        services.AddScoped<IAuth0Manager, Auth0Manager>();
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