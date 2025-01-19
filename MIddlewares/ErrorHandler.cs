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
            await HandleExceptionAsync(context, ex); // Lida com a exceção
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, GenericApiError exception)
    {
        // Define um código de status padrão para erros genéricos
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

        var response = new ErrorResponse()
        {
            message = exception.Message,
            status = exception.StatusCode,
        };
        
        return context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Console.WriteLine(exception.Message);
        var errorResponse = new
        {
            StatusCode = 500,
            Message = "Erro interno!",
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