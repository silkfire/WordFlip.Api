namespace Wordsmith.WordFlip.WebApi.Utils
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Routing;

    using System.Net;
    using System.Threading.Tasks;


    public static class ControllerUtilities
    {
        public static IActionResult RespondWithJsonError(this ControllerBase controller, HttpStatusCode statusCode, string message)
        {
            return controller.StatusCode((int)statusCode, new ErrorResult(message));
        }

        public static async Task RespondWithJsonError(this HttpContext context, string message)
        {
            await new JsonResult(new ErrorResult(message)).ExecuteResultAsync(new ActionContext(context, context.GetRouteData(), new ActionDescriptor()));
        }


        private class ErrorResult
        {
            public string Error { get; }

            public ErrorResult(string error)
            {
                Error = error;
            }
        }
    }
}
