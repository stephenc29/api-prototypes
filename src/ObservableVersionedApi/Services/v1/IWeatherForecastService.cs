
using ObservableVersionedApi.Models.v1;

namespace ObservableVersionedApi.Services.v1
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}