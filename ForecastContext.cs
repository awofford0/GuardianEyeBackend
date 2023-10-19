using Microsoft.EntityFrameworkCore;

namespace aspnetbackend
{
    public class ForecastContext : DbContext
    {
        public ForecastContext(DbContextOptions<ForecastContext> options)
    : base(options)
        {
        }

        public DbSet<WeatherForecast> ForecastList { get; set; } = null!;
    }
}
