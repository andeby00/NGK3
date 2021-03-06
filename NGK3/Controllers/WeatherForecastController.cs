using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp;
using NGK3.Data;
using NGK3.Data.Models;
using NGK3.Hubs;

namespace NGK3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        IHubContext<WeatherHub> _weatherHubContext;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private DbViews _db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext context, IHubContext<WeatherHub> hub)
        {
            _weatherHubContext = hub;
            _logger = logger;
            _db = new DbViews(context);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                Location = new Location{Name = "Chokoladen", Lat = 56.17, Lon = 10.18 },
                TemperatureC = rng.Next(-20, 55),
                Humidity = rng.Next(0, 101),
                AirPressure = rng.Next(980, 1030)
                    
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult InsertDummyData()
        {
            try
            {
                var rng = new Random();
                var var = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        Location = new Location {Name = "Chokoladen", Lat = 56.17, Lon = 10.18},
                        TemperatureC = rng.Next(-20, 55),
                        Humidity = rng.Next(0, 101),
                        AirPressure = rng.Next(980, 1030)

                    })
                    .ToArray();

                _db.SeedDummy(var);

                return StatusCode(201);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Latests")]
        public async Task<ActionResult<List<WeatherForecast>>> GetLatestForecasts()
        {
            return await _db.GetLatestForecasts();
        }

        [HttpGet("Latest")]
        public async Task<ActionResult<WeatherForecast>> GetLatestForecast()
        {
            return await _db.GetLatestForecast();
        }
        
        [HttpGet("{date}")]
        public async Task<ActionResult<List<WeatherForecast>>> GetWeatherForecasts(DateTime date)
        {
            return await _db.GetForecastsBy(date);
        }
        
        [HttpGet("{startdate}/{enddate}")]
        public async Task<ActionResult<List<WeatherForecast>>> GetWeatherForecasts(DateTime startdate, DateTime enddate)
        {
            return await _db.GetForecastsBetween(startdate, enddate);
        }

        [HttpPost("GenerateForecast")]
        public async Task<IActionResult> GenerateForecast(WeatherForecast forecast)
        {
            try
            {
                _db.SeedForecast(forecast);

                await _weatherHubContext.Clients.All.SendAsync("liveUpdate");

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
