
using HelloWorldApi.Models.v1;

namespace HelloWorldApi.Services.v1
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}