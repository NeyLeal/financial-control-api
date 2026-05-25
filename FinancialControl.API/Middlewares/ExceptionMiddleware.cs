using FinancialControl.Application.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FinancialControl.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger
            ) 
        {  
            _next = next; 
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try             {
                await _next(context);
            }
            catch(BusinessException ex) 
            { 
                _logger.LogWarning(ex,
                    "Busines exception on {Path}",
                    context.Request.Path);

                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception on {Path}",
                    context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var response = "Internal server error";
                await context.Response.WriteAsJsonAsync(response);
            }
            //catch (Exception exception)
            //{
            //    context.Response.StatusCode = 500;

            //    await context.Response.WriteAsJsonAsync(new
            //    {
            //        message = exception.Message,
            //        stackTrace = exception.StackTrace
            //    });
            //}
        }
    }
}
