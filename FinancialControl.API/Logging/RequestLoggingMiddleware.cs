using System.Diagnostics;

namespace FinancialControl.API.Logging
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware (
            RequestDelegate next, 
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = Stopwatch.StartNew();

            try
            {
                await _next(context);
                watch.Stop();

                _logger.LogInformation("HTTP {Method} {Path} -> {StatusCode} ({Elapsed}ms)",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                watch.Stop();
                _logger.LogError(
                    ex,
                    "HTTP {Method} {Path} FAILED ({Elapsed}ms)",
                    context.Request.Method,
                    context.Request.Path,
                    watch.ElapsedMilliseconds
                    );
                throw;
            }
        }
    }
}
