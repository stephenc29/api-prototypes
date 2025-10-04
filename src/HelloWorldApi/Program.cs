using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter(); // Export to console for now
    });

builder.Services.AddControllers();
builder.Services.AddApiVersioning().AddMvc();
builder.Services.AddSingleton<HelloWorldApi.Services.v1.IWeatherForecastService, HelloWorldApi.Services.v1.WeatherForecastService>();
builder.Services.AddSingleton<HelloWorldApi.Services.v2.IWeatherForecastService, HelloWorldApi.Services.v2.WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
