using System.Text.Json;
using Talabat.Api.Errors;

namespace Talabat.Api.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment host;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger ,IHostEnvironment host ) {
            this.next = next;
            this.logger = logger;
            this.host = host;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next.Invoke(httpContext);

            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 500;
                var res = host.IsDevelopment()?new ApiExceptionResponse(500,e.Message,e.StackTrace):
                new ApiExceptionResponse(500);
                var option = new JsonSerializerOptions() {PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
               
                var json=JsonSerializer.Serialize(res,option);
                await  httpContext.Response.WriteAsync(json);

            }

        }
    }
}
