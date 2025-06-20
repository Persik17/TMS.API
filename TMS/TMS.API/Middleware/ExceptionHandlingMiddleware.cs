using System.Net;

namespace TMS.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally and providing a consistent error response.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        /// <param name="logger">The logger for logging exceptions.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="next"/> or <paramref name="logger"/> is <c>null</c>.</exception>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the exception and writes an appropriate error response to the HTTP context.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="exception">The exception that occurred.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = "Internal Server Error";

            switch (exception)
            {
                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
                case UnauthorizedAccessException authEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = "Unauthorized";
                    break;
                //Consider adding new Exception Types to consider.
                default:
                    _logger.LogError(exception, "An unhandled exception occurred.");
                    break;
            }

            _logger.LogError(exception, message);

            var errorResponse = new
            {
                context.Response.StatusCode,
                Message = message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
