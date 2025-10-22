using Asp.Versioning;
using ObservableVersionedApi.Models.v2;
using ObservableVersionedApi.Services.v2;
using Microsoft.AspNetCore.Mvc;

namespace ObservableVersionedApi.Controllers.v2
{
    [ApiVersion( 2.0 )]
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
