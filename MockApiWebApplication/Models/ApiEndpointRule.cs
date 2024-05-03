﻿using System.Text.Json;

namespace MockApiWebApplication.Models;

public class ApiEndpointRule
{
    public string HttpMethod { get; set; } = default!;
    public string UrlEndpointPath { get; set; } = default!;
    public int? HttpStatusCode { get; set; }
    public int? LatencyInSec { get; set; }
    public JsonDocument JsonSchema { get; set; } = default!;
}