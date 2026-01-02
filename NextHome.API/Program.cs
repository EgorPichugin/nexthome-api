using System.Text;
using Microsoft.IdentityModel.Tokens;
using NextHome.API.Extensions;

namespace NextHome.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.ConfigureServices(builder.Configuration);
        
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.ConfigureCors();
        }
        var app = builder.Build();
        app.ConfigureApp();
        app.Run();
    }
}
