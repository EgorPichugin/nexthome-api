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
    /// <returns>The configured WebApplication instance.</returns>
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(EnvironmentManager.CorsPolicyName);
            app.UseHttpsRedirection();
        }
        
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}