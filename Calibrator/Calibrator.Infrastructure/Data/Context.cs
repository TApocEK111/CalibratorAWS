using Calibrator.Domain.Model.Setpoint;
using Calibrator.Domain.Model.Report;
using Microsoft.EntityFrameworkCore;

namespace Calibrator.Infrastructure.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public DbSet<Report> Reports { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorChannel> SensorChannels { get; set; }
    public DbSet<Sample> Samples{ get; set; }
    public DbSet<AverageSample> AverageSamples{ get; set; }
    public DbSet<ExternalImpact> ExternalImpacts { get; set; }
    
    public DbSet<Setpoint> Setpoints { get; set; }
    public DbSet<Exposure> Exposures{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>().Ignore(s => s.Error);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ReportDB;Username=postgres;Password=admin");
    }
}
