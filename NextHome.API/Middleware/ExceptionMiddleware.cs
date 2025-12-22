namespace NextHome.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ArgumentNullException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { message = "Internal error" });
        }
    }
}