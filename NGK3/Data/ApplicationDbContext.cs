using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NGK3.Data.Models;

namespace NGK3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NGK3.Data.Models.User> Users { get; set; }
        public DbSet<NGK3.Data.Models.WeatherForecast> WeatherForecasts { get; set; }
    }
}
