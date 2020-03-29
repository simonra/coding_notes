using System.Web;
using System.Web.HttpContext;
using Services;

namespace CustomMiddleware
{
    public class SetCustomHttpStatusCodeMiddleware
    {
        private RequestDelegate _next;
        private HttpStatusCodeStateKeepingService _httpStatusCodeStateKeepingService;

        public SetCustomHttpStatusCodeMiddleware(RequestDelegate next, HttpStatusCodeStateKeepingService httpStatusCodeStateKeepingService)
        {
            _next = next;
            _httpStatusCodeStateKeepingService = httpStatusCodeStateKeepingService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Set up callback that will get called once a response to the client is beginning to be sent.
            // This is the point in time that you have to hook in to if you want to manipulate for intance the headers of the request before it's sent back to the client.
            // That is because (as of this date) the response is being streamed back to the client as it is being written.
            // And because the headers are at the beginning of the response/being sent first, you have to be the first to wrtie the value if you want your value to be the one the client gets.
            context.Response.OnStarting((state) =>
            {
                // ToDo: Manipulate response status code based on value of _httpStatusCodeStateKeepingService.StatusCode
                context.Response.StatusCode = HttpStatusCode.OK; // Everything is fine. Always.
                return Task.CompletedTask;
            }, null);

            // Let the next middleware down the line do it's thing.
            // This gets executed before the callback above.
            await _next.Invoke(context);
        }
    }

    public static class SetCustomHttpStatusCodeMiddlewareExtensions
    {
        public static IApplicationBuilder UseSetCustomHttpStatusCodeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetCustomHttpStatusCodeMiddleware>();
        }
    }
}

