using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]                                                                                                      // signifies this class is of type API
    [Route("[controller]")]           // is replaced with WeatherForecastController as this is a placeholder // https://localhost:5001/weatherforecast // route of API / how user gets to API controller from client
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot"                                         // removed "sweltering", "scortching" 
        };

        private readonly ILogger<WeatherForecastController> _logger;

                                                                                                                    // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
                                                                                                                            // we will create our own controllers and enpoints
        [HttpGet]                                                                                                        // controller endpoint // GET request from browser  to API // returns code results to an .ToArray()
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();                                                                         // returns a list of weather forecasts 
        }
    }
}
