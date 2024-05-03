using MockApiWebApplication.Models;
using MockContentGenerator;

namespace MockApiWebApplication.Middleware;

public class ApiDictionaryRoutingMiddleware
{
    private readonly ApplicationCache<ApiEndpointRule> _memoryCache;
    private readonly RequestDelegate _next;
    private readonly ContentGenerator _contentGenerator;

    public ApiDictionaryRoutingMiddleware(RequestDelegate next, ApplicationCache<ApiEndpointRule> memoryCache)
    {
        _next = next;
        _memoryCache = memoryCache;
        _contentGenerator = new ContentGenerator();
    }

    public async Task Invoke(HttpContext context)
    {
        // from Swagger Registry Route ? skip it
        if (context.Request.Method == "POST" && context.Request.Path == "/RegistryRoutes")
        {
            await _next(context);
            return;
        }

        bool isMessageBodyReceiverConsumerExist =
            _memoryCache.GetByKey($"{context.Request.Method}-{context.Request.Path}", out ApiEndpointRule apiValuedRoute);

        if (isMessageBodyReceiverConsumerExist)
        {
            if (apiValuedRoute.LatencyInSec.HasValue)
            {
                await Task.Delay(apiValuedRoute.LatencyInSec.Value * 1000); // in sec
            }
            context.Response.StatusCode = apiValuedRoute.HttpStatusCode ?? 200;
            context.Response.ContentType = "application/json; charset=utf-8";
            var jsonGeneratedContent = _contentGenerator.GenerateBySchema(apiValuedRoute.JsonSchema);
            await context.Response.WriteAsync(jsonGeneratedContent);
            return;
        }

        await _next(context);
    }
}
