using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NGK3.Controllers;
using NGK3.Data;
using NGK3.Data.Models;
using NGK3.Hubs;
using NSubstitute;
using NUnit.Framework;

namespace NGK3.Tests
{
    public class Tests
    {

        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _hub;
        private ILogger<WeatherForecastController> _logger;
        private WeatherForecastController _controller;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(CreateDbConnection()).Options);

            static DbConnection CreateDbConnection()
            {
                var conn = new SqliteConnection("filename=:memory:");
                conn.Open();
                return conn;
            }

            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _hub = Substitute.For<IHubContext<WeatherHub>>();
            _logger = Substitute.For<ILogger<WeatherForecastController>>();
            _controller = new WeatherForecastController(_logger, _context, _hub);

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

            _context.Database.EnsureCreated();
            _context.AddRange(var);
            _context.SaveChanges();
        }

        [Test]
        public void Test_Controller_GetLatestForecasts()
        {

            var expected = _context.WeatherForecasts.Include(f => f.Location).Take(3)
                .OrderByDescending(forecast => forecast.Date).ToListAsync().Result;

            var actual = _controller.GetLatetestForecasts().Result.Value;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(6)]
        public void Test_Controller_GetForecastsFor(int var)
        {
            var date = DateTime.Now.AddDays(var);
            var expected = _context.WeatherForecasts
                .Where(f => f.Date.Year == date.Year & f.Date.Month == date.Month & f.Date.Day == date.Day)
                .ToListAsync().Result;

            var actual = _controller.GetWeatherForecasts(DateTime.Now.AddDays(var)).Result.Value;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(0, 5)]
        [TestCase(3, 4)]
        [TestCase(0, 3)]
        public void Test_Controller_GetForecastsbetween(int var, int var2)
        {
            var startdate = DateTime.Now.AddDays(var);
            var enddate = DateTime.Now.AddDays(var2);
            var expected = _context.WeatherForecasts.Include(forecast => forecast.Location)
                .Where(forecast => startdate <= forecast.Date && forecast.Date <= enddate).ToListAsync().Result;

            var actual = _controller.GetWeatherForecasts(DateTime.Now.AddDays(var), DateTime.Now.AddDays(var2)).Result
                .Value;

            Assert.AreEqual(expected, actual);
        }
    }
}