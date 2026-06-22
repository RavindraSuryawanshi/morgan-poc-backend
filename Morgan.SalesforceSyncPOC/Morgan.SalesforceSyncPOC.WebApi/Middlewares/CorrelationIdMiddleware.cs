using Serilog.Context;

namespace Morgan.SalesforceSyncPOC.WebApi.Middlewares
{
    /// <summary>
    /// Middleware that adds a correlation identifier to each request and response.
    /// The correlation ID is pushed into the Serilog logging context so all log
    /// entries generated during the request can be traced end-to-end.
    /// </summary>
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Extracts the request correlation ID, enriches the Serilog context,
        /// adds the correlation ID to the response headers, and invokes the next
        /// middleware in the pipeline.
        /// </summary>
        public async Task Invoke(
            HttpContext context)
        {
            var correlationId =
                context.TraceIdentifier;

            using (LogContext.PushProperty(
                "CorrelationId",
                correlationId))
            {
                context.Response.Headers.Add(
                    "X-Correlation-Id",
                    correlationId);

                await _next(context);
            }
        }
    } 
}