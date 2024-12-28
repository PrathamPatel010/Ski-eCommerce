using System.Net;
using System.Text.Json;
using API.Error;

namespace API.Middleware
{
    public class ExceptionMiddleware(IHostEnvironment env,RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            } 
            catch(Exception e)
            {
                await HandleExceptionAsync(e,context,env);
            }
        }

        private static Task HandleExceptionAsync(Exception e, HttpContext context,IHostEnvironment env)
        {
            context.Response.ContentType="application/json";
            context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
            var response = env.IsDevelopment()
            ? new ApiErrorResponse(context.Response.StatusCode,e.Message,e.StackTrace)
            : new ApiErrorResponse(context.Response.StatusCode,e.Message,"Internal Server Error");
            var options = new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
            var json = JsonSerializer.Serialize(response,options);
            return context.Response.WriteAsync(json);
        }
    }
}