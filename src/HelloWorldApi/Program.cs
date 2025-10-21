using HelloWorldApi.Extensions;
using HelloWorldApi.Swagger;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiMetrics();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HelloWorldApi"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddApiMetrics()
            .AddPrometheusExporter(); // Export to /metrics
    })
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("HelloWorldApi"))
            .AddAspNetCoreInstrumentation(options =>
            {
                options.EnrichWithHttpRequest = (activity, request) =>
                {
                    var apiVersion = request.HttpContext.GetRequestedApiVersion()?.ToString() ?? "unknown";
                    activity.SetTag("api_version", apiVersion);
                };
                options.EnrichWithHttpResponse = (activity, response) =>
                {
                    var apiVersion = response.HttpContext.GetRequestedApiVersion()?.ToString() ?? "unknown";
                    activity.SetTag("api_version", apiVersion);
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

app.UseRequestMetrics();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
