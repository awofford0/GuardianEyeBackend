using Microsoft.EntityFrameworkCore;

namespace aspnetbackend
{
    public class DetectionContext : DbContext
    {
        public DetectionContext(DbContextOptions<DetectionContext> options)
    : base(options)
        {
        }

        public DbSet<Detection> Detections { get; set; } = null!;
    }
}
