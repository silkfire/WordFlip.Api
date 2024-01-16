namespace Wordsmith.WordFlip.WebApi.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Routing;

    using System.Net;
    using System.Threading.Tasks;

    public static class ApiExtensions
    {
        public static IActionResult RespondWithJsonError(this ControllerBase controller, HttpStatusCode statusCode, string message)
        {
            return controller.StatusCode((int)statusCode, new ErrorResult(message));
        }

        public static async Task RespondWithJsonError(this HttpContext context, string message)
        {
            await new ObjectResult(new ErrorResult(message)).ExecuteResultAsync(new ActionContext(context, context.GetRouteData(), new ActionDescriptor()));
        }

        private record ErrorResult(string Error);
    }
}
