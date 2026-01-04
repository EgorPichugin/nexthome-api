using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NextHome.API.Extensions;

namespace NextHome.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var envOptions = builder.Configuration.Get<EnvironmentOptions>()
                         ?? throw new InvalidOperationException("EnvironmentOptions not found");
        
        builder.Services.ConfigureServices(builder.Configuration);
        builder.Services.ConfigureCors(envOptions);
        
        var app = builder.Build();
        app.ConfigureApp(envOptions);
        app.Run();
    }
}
