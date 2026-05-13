using SpaceXProject.API.Exceptions;
using System.Text.Json;

namespace SpaceXProject.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (EmailAlreadyExistsException ex)
            {
                await WriteJson(context, StatusCodes.Status409Conflict, new
                {
                    title = "Email already in use",
                    status = 409,
                    detail = ex.Message
                });
            }
            catch (InvalidCredentialsException ex)
            {
                await WriteJson(context, StatusCodes.Status401Unauthorized, new
                {
                    title = "Authentication failed",
                    status = 401,
                    detail = ex.Message
                });
            }
            catch (SpaceXApiException ex)
            {
                await WriteJson(context, StatusCodes.Status502BadGateway, new
                {
                    title = "Service unavailable",
                    status = 502,
                    detail = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteJson(context, StatusCodes.Status500InternalServerError, new
                {
                    title = "An unexpected error occurred",
                    status = 500
                });
            }
        }

        private static Task WriteJson(HttpContext context, int statusCode, object body)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }

}
