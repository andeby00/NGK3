using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        public List<WeatherForecast> GetLatestForecasts()
        {
            var temp = from weatherForecasts in _context.WeatherForecasts 
                orderby weatherForecasts.Date
                select weatherForecasts;

            var temp2 = temp.ToArray();

            var var = new List<WeatherForecast>();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var.Add(temp2[i]);
                }
                catch
                {
                    break;
                }
            }

            return var;
        }
    }
}
