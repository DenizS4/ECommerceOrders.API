namespace ProductsAPI.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            //Log the exception
            _logger.LogError($"{ex.GetType()}: {ex.Message}");

            if (ex.InnerException is not null)
            {
                //Log the inner exception
                _logger.LogError($"{ex.InnerException.GetType()}: {ex.InnerException.Message}");
            }

            context.Response.StatusCode = 500; //Internal Server Error
            
            await context.Response.WriteAsJsonAsync(new {Messsage = ex.Message, Type = ex.GetType().ToString()});
        }
    }
   
}
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}