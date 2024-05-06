using System.Text.Json;

namespace MockApiWebApplication.Models;

public class ApiEndpointRule
{
    public string HttpMethod { get; set; } = default!;
    public string UrlEndpointPath { get; set; } = default!;
    public string? ResponseMessage { get; set; } = default!;
    public int? HttpStatusCode { get; set; }
    public int? LatencyInMilliseconds { get; set; }
    public bool? IsAuthorizationValidate { get; set; }
    public string? Authority { get; set; }
    public JsonDocument? JsonSchema { get; set; } = default!;
}
