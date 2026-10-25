using System.Text.Json;
using ProductCatalog.Models.Responses;

namespace ProductCatalog.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();

            var (statusCode, message) = ex switch
            {
                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound,
                        "Resource not found"),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest,
                        "Invalid request"),

                _ =>
                    (StatusCodes.Status500InternalServerError,
                        "An unexpected error occurred")
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = BaseResponse<object>.ErrorResponse(
                message,
                statusCode == StatusCodes.Status500InternalServerError
                    ? null
                    : new object[] { ex.Message });

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}