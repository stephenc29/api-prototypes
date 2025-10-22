using ObservableVersionedApi.Models.v2;

namespace ObservableVersionedApi.Services.v2
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}