using HelloWorldApi.Metrics;
using HelloWorldApi.Middleware;
using OpenTelemetry.Metrics;

namespace HelloWorldApi.Extensions;

public static class ApiMetricsExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services)
    {
        services.AddSingleton<ApiMetrics>();
        return services;
    }

    public static MeterProviderBuilder AddApiMetrics(this MeterProviderBuilder builder)
    {
        return builder.AddMeter(ApiMetrics.MeterName);
    }
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRequestMetrics(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestMetricsMiddleware>();
    }
}