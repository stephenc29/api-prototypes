using Asp.Versioning;
using ObservableVersionedApi.Models.v1;
using ObservableVersionedApi.Services.v1;
using Microsoft.AspNetCore.Mvc;

namespace ObservableVersionedApi.Controllers.v1
{
    [ApiVersion( 1.0 )]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecastService.GetForecasts();
        }
    }
}
