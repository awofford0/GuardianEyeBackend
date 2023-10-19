using System.ComponentModel.DataAnnotations;

namespace aspnetbackend
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        [Key]
        public int Id { get; set; }
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}