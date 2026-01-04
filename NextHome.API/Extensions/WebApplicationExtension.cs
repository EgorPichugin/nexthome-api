using NextHome.API.Constants;
using NextHome.API.Middleware;

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
    /// <param name="envOptions">Allows to retrieve environment variables.</param>
    /// <returns>The configured WebApplication instance.</returns>
    public static WebApplication ConfigureApp(this WebApplication app, EnvironmentOptions envOptions)
    {
        if (envOptions.ENABLE_SWAGGER)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{SwaggerDocs.ApiVersion}/swagger.json", SwaggerDocs.ApiName);
            });
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(envOptions.CORS_POLICY_NAME);
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}