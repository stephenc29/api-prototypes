using ObservableVersionedApi.Metrics;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ObservableVersionedApi.Middleware;

public class RequestMetricsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiMetrics _apiMetrics;

    public RequestMetricsMiddleware(RequestDelegate next, ApiMetrics apiMetrics)
    {
        _next = next;
        _apiMetrics = apiMetrics;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Record custom metric after request is processed
        var apiVersion = context.GetRequestedApiVersion()?.ToString() ?? "unknown";
        var route = GetRouteTemplate(context);
        var method = context.Request.Method;
        var statusCode = context.Response.StatusCode;

        _apiMetrics.RecordRequest(apiVersion, route, method, statusCode);
    }

    private static string GetRouteTemplate(HttpContext context)
    {
        return context.GetEndpoint()?.Metadata
            ?.GetMetadata<ControllerActionDescriptor>()
            ?.AttributeRouteInfo?.Template 
            ?? context.Request.Path.ToString();
    }
}