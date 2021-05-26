using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NGK3.Data.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Location Location { get; set; }
        [Column(TypeName = "decimal(10,1)")]
        public double TemperatureC { get; set; }
        public int Humidity { get; set; }
        [Column(TypeName = "decimal(10,1)")]
        public double AirPressure { get; set; }
    }
}
