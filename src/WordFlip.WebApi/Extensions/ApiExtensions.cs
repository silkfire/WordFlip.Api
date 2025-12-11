namespace Wordsmith.WordFlip.WebApi.Extensions;

using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

public static class ApiExtensions
{
    public static async Task RespondWithJsonError(this HttpContext context, string message)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new ErrorResult { Message = message });
    }

    private class ErrorResult
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public required string Message { get; init; }
    }
}
