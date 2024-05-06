using MockApi.ContentGenerator;
using MockApiWebApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace MockApiWebApplication.Middleware;

public class ApiDictionaryRoutingMiddleware
{
    private readonly ApplicationCache<ApiEndpointRule> _memoryCache;
    private readonly RequestDelegate _next;
    private readonly JsonSchemaBuilder _jsonSchemaBuilder;

    public ApiDictionaryRoutingMiddleware(RequestDelegate next, ApplicationCache<ApiEndpointRule> memoryCache)
    {
        _next = next;
        _memoryCache = memoryCache;
        _jsonSchemaBuilder = new JsonSchemaBuilder();
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
            if (apiValuedRoute.LatencyInMilliseconds.HasValue)
            {
                await Task.Delay(apiValuedRoute.LatencyInMilliseconds.Value); // in Milliseconds
            }            
            // authorization validation ?
            if (apiValuedRoute.IsAuthorizationValidate.GetValueOrDefault())
            {
                var authValidated = AuthorizationValidate(context.Request, apiValuedRoute);
                if (!authValidated.isValid)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(authValidated.errorMessage!);
                    return;
                }
            }

            context.Response.StatusCode = apiValuedRoute.HttpStatusCode ?? 200;
            context.Response.ContentType = "application/json; charset=utf-8";
            if (!string.IsNullOrEmpty(apiValuedRoute.ResponseMessage))
            {
                await context.Response.WriteAsync(apiValuedRoute.ResponseMessage!);
                return;
            }
            var jsonGeneratedContent = _jsonSchemaBuilder.Build(apiValuedRoute.JsonSchema.RootElement.ToString()!);
            await context.Response.WriteAsync(jsonGeneratedContent.jsonSerializedContent);
            return;
        }

        await _next(context);
    }

    private (bool isValid, string? errorMessage) AuthorizationValidate(HttpRequest request, ApiEndpointRule apiValuedRoute)
    {
        bool isValid = true;
        string? errorMessage = default;
        var authToken = request.Headers.Authorization.ToString();
        if(string.IsNullOrEmpty(authToken))
        {
            isValid = false;
            errorMessage = "Authorization Header is missed";
        }
        else
        {
            if(authToken.StartsWith("Bearer "))
            {
                var jwtToken = new JwtSecurityToken(authToken.Substring(7));
                if(jwtToken.Issuer != apiValuedRoute.Authority)
                {
                    isValid = false;
                    errorMessage = "Authorization Jwt Token Authority incorrect";
                }
            }
        }
        
        return (isValid, errorMessage);
    }
}
