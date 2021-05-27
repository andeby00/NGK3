using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NGK3.Data.Models;

namespace NGK3.Data
{
    public class DbViews
    {
        private readonly ApplicationDbContext _context;

        public DbViews(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Task<List<WeatherForecast>>  GetLatestForecasts()
        {
            return _context.WeatherForecasts.Include(f => f.Location).Take(3).OrderByDescending(forecast => forecast.Date).ToListAsync();
        }

        public Task<List<WeatherForecast>> GetForecastsBy(DateTime date)
        {
            return _context.WeatherForecasts.Where(f => f.Date.Year == date.Year & f.Date.Month == date.Month & f.Date.Day == date.Day).ToListAsync();
        }

        public Task<List<WeatherForecast>> GetForecastsBetween(DateTime startdate, DateTime enddate)
        {
            return _context.WeatherForecasts.Include(forecast => forecast.Location).Where(forecast => startdate <= forecast.Date && forecast.Date <= enddate).ToListAsync();
        }

        public void SeedDummy(WeatherForecast[] list)
        {
            _context.AddRange(list);
            _context.SaveChanges();
        }

        public void SeedForecast(WeatherForecast forecast)
        {
            _context.Add(forecast);
            _context.SaveChanges();
        }
    }
}
