using System.Net;
using System.Text.Json;

namespace Morgan.SalesforceSyncPOC.WebApi.Middlewares
{
    /// <summary>
    /// Global exception handling middleware that captures unhandled exceptions,
    /// logs them, and returns a standardized error response.
    /// </summary>
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Executes the next middleware in the pipeline and handles any
        /// unhandled exceptions by returning an HTTP 500 response.
        /// </summary>
        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var correlationId =
                    context.TraceIdentifier;

                _logger.LogError(ex, "Unhandled exception occurred. CorrelationId: {CorrelationId}",correlationId);

                context.Response.Clear();

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType =
                    "application/json";

                var response = new ErrorResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }

        /// <summary>
        /// Represents the standard error payload returned to API consumers.
        /// </summary>
        private sealed class ErrorResponse
        {

            public int StatusCode { get; set; }

            public string Message { get; set; } = string.Empty;
        }
    }
}