using asp_rest_model.Helpers;
using asp_rest_model.Models;

namespace asp_rest_model.MIddlewares;


public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // segue o fluxo
        }
        catch (GenericApiError ex)
        {
            await HandleExceptionAsync(context, ex);
        }

        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // Lida com a exceção (2 com difetentes tipos)
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, GenericApiError exception)
    {
        var statusCode = exception.StatusCode;

        var errorResponse = new
        {
            StatusCode = statusCode,
            Message = exception.Message,
            // Detailed = exception.Message // Remova para evitar expor detalhes sensíveis em produção
        };

        // Formata a resposta como JSON
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsJsonAsync(errorResponse);
    }

    // Expecition normal (geral)
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Console.WriteLine(exception.Message);
        var errorResponse = new
        {
            StatusCode = 500,
            Message = "Internal Server Error",
            // Detailed = exception.Message // Remova para evitar expor detalhes sensíveis em produção
        };

        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}

public static class GlobalErrorHandlerMiddlewareExtends
{
    public static IApplicationBuilder UseGlobalErrorHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
    }
}