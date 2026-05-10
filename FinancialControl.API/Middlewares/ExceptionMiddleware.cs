using FinancialControl.Application.Exceptions;
using System.Text.Json;

namespace FinancialControl.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) {  _next = next; }
        public async Task Invoke(HttpContext context)
        {
            try             {
                await _next(context);
            }
            catch(BusinessException ex) 
            { 
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";
                var response = new { message = ex.Message };
                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var response = "Internal server error";
                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
