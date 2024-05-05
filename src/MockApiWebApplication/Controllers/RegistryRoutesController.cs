using Microsoft.AspNetCore.Mvc;
using MockApiWebApplication.Models;

namespace MockApiWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistryRoutesController : ControllerBase
    {
        private readonly ILogger<RegistryRoutesController> _logger;
        private readonly ApplicationCache<ApiEndpointRule> _memoryCache;

        public RegistryRoutesController(ApplicationCache<ApiEndpointRule> memoryCache, ILogger<RegistryRoutesController> logger)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet(Name = "RegisteredEndpoint")]
        public IDictionary<string, ApiEndpointRule> RegisteredEndpoint()
        {
            return _memoryCache.GetAll();
        }

        [HttpPost(Name = "RegistryNewEndpoint")]
        public IActionResult RegistryNewEndpoint([FromBody] ApiEndpointRule registryNewApiEndpoint)
        {
            _memoryCache.Set($"{registryNewApiEndpoint.HttpMethod}-{registryNewApiEndpoint.UrlEndpointPath}", registryNewApiEndpoint);

            _logger.LogInformation($"add new route rule with pattern: {registryNewApiEndpoint.HttpMethod}-{registryNewApiEndpoint.UrlEndpointPath}");

            var schema = registryNewApiEndpoint.JsonSchema;
            return Ok($"registered new schema {schema} with url pattern {registryNewApiEndpoint.HttpMethod}-{registryNewApiEndpoint.UrlEndpointPath}");
        }
    }
}
