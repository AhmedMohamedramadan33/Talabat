using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Repository.Data.Repositories;

namespace Talabat.Api.Helpers
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int num;

        public CacheAttribute(int num) {
            this.num = num;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var responsecache=context.HttpContext.RequestServices.GetRequiredService<ICacheResponseService>();
            var cachekey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var response = await responsecache.Getcachedresponse((string)cachekey);
            if(!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult()
                {
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }
            var excutedactioncontext = await next.Invoke();
            if(excutedactioncontext.Result is OkObjectResult okobj && okobj is not null)
            {
                await responsecache.CacheResponse((string)cachekey, okobj.Value, TimeSpan.FromSeconds(num));
            }
        }

        private object GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keybuilder = new StringBuilder();
            keybuilder.Append(request.Path);
            foreach(var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                keybuilder.Append($"|{key}-{value}");
            }
            return keybuilder.ToString();
        }
    }
}
