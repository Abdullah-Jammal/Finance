using System.Text.Json;
using Finance.API.Models.Queries.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Finance.API.Authorization;

public sealed class AuthorizationResultHandler
    : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status401Unauthorized,
                "Unauthorized");
            return;
        }

        if (authorizeResult.Forbidden)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status403Forbidden,
                "Forbidden");
            return;
        }

        await defaultHandler.HandleAsync(
            next,
            context,
            policy,
            authorizeResult);
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        int statusCode,
        string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
