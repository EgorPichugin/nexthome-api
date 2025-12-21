namespace NextHome.API.Extensions;

public static class WebApplicationExtension
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(EnvironmentManager.CorsPolicyName);
            app.UseHttpsRedirection();
        }
        
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}