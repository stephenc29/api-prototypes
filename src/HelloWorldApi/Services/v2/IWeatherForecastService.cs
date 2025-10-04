using HelloWorldApi.Models.v2;

namespace HelloWorldApi.Services.v2
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}