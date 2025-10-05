using HelloWorldApi.Swagger;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HelloWorldApi"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddPrometheusExporter(); // Export to /metrics
    })
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HelloWorldApi"))
            .AddAspNetCoreInstrumentation(options => {
                options.EnrichWithHttpResponse = (activity, response) =>
                {
                    var routePattern = response.HttpContext.GetEndpoint()?.Metadata
                        .GetMetadata<ControllerActionDescriptor>()?.AttributeRouteInfo?.Template ?? response.HttpContext.Request.Path;

                    if (!routePattern.Contains("{version:apiVersion}"))
                    {
                        return;
                    }

                    var apiVersion = response.HttpContext.GetRequestedApiVersion()?.ToString() ?? "unknown";

                    var resolvedRoute = routePattern.Replace("{version:apiVersion}", apiVersion);

                    activity.SetTag("http.route", resolvedRoute);
                    activity.DisplayName = activity.DisplayName.Replace("{version:apiVersion}", apiVersion);
                };
            })
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console for now
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning()
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                });

builder.Services.AddSingleton<HelloWorldApi.Services.v1.IWeatherForecastService, HelloWorldApi.Services.v1.WeatherForecastService>();
builder.Services.AddSingleton<HelloWorldApi.Services.v2.IWeatherForecastService, HelloWorldApi.Services.v2.WeatherForecastService>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

// Configure the HTTP request pipeline.
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
