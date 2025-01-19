namespace asp_rest_model.Helpers;

public class GenericApiError(string message, int statusCode = 400) : Exception
{
    public new readonly string Message = message;
    public readonly int StatusCode = statusCode;
}