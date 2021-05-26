using System;

namespace NGK3.Data.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Location Location { get; set; }
        public int TemperatureC { get; set; }
        public int Humidity { get; set; }
        public double AirPressure { get; set; }
    }
}
