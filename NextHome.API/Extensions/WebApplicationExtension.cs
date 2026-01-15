using Microsoft.Extensions.Options;
using NextHome.API.Constants;
using NextHome.API.Middleware;
using NextHome.API.Options;
using NextHome.Core.Interfaces;

namespace NextHome.API.Extensions;

/// <summary>
/// Web application extension.
/// </summary>
public static class WebApplicationExtension
{
    /// <summary>
    /// Configures the application pipeline for the WebApplication instance.
    /// </summary>
    /// <param name="app">The WebApplication instance to configure.</param>
    /// <returns>The configured WebApplication instance.</returns>
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        var swaggerOptions = app.Services
            .GetRequiredService<IOptions<SwaggerOptions>>()
            .Value;
        var corsOptions = app.Services
            .GetRequiredService<IOptions<CorsOptions>>()
            .Value;
        
        if (swaggerOptions.Enable)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{SwaggerDocs.ApiVersion}/swagger.json", SwaggerDocs.ApiName);
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(corsOptions.PolicyName);
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}