using System.Net;
using System.Text.Json;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Exceptions;

namespace EnterpriseHR.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (CommonExceptions ex)
        {
            _logger.LogWarning(ex, "Business rule violation: {Message}", ex.Message);
            await WriteErrorResponseAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await WriteErrorResponseAsync(
                context,
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred while processing the request.");
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = APIResponse<object>.Fail(message);
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}