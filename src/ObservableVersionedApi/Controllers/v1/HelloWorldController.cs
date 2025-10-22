using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ObservableVersionedApi.Controllers.v1
{
    [ApiVersion(1.0)]
    [ApiVersion(1.1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello, World!";
        }

        [HttpGet, MapToApiVersion("1.1")]
        public string GetV1_1()
        {
            return "Hello, World! This is version 1.1.";
        }
    }
}
