var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning().AddMvc();
builder.Services.AddSingleton<HelloWorldApi.Services.v1.IWeatherForecastService, HelloWorldApi.Services.v1.WeatherForecastService>();
builder.Services.AddSingleton<HelloWorldApi.Services.v2.IWeatherForecastService, HelloWorldApi.Services.v2.WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
