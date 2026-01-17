using Finance.API.Models;
using Finance.Application.Common.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace Finance.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Validation failed",
                errors: ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status404NotFound,
                ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status401Unauthorized,
                "Unauthorized");
        }
        catch (Exception)
        {
            await WriteErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred");
        }
    }

    private static async Task WriteErrorAsync(
        HttpContext context,
        int statusCode,
        string message,
        object? errors = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
