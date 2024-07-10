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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sample>().Ignore(s => s.Error);

        //modelBuilder.Entity<Report>().HasMany(r => r.Sensors).WithOne(s => s.Report).HasForeignKey(s => s.Report);
        //modelBuilder.Entity<Sensor>().HasMany(s => s.Channels).WithOne(c => c.Sensor).HasForeignKey(c => c.Sensor);
        //modelBuilder.Entity<SensorChannel>().HasMany(c => c.Samples).WithOne(s => s.Channel).HasForeignKey(c => c.Channel);
        //modelBuilder.Entity<SensorChannel>().HasMany(c => c.AvgSamples).WithOne(s => s.Channel).HasForeignKey(s => s.Channel);
        //modelBuilder.Entity<Sample>().HasMany(s => s.ExternalImpacts).WithOne(i => i.Sample).HasForeignKey(i => i.Sample);

        modelBuilder.Entity<Report>().HasMany(r => r.Sensors).WithOne(s => s.Report).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Sensor>().HasMany(s => s.Channels).WithOne(c => c.Sensor).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SensorChannel>().HasMany(c => c.Samples).WithOne(s => s.Channel).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SensorChannel>().HasMany(c => c.AvgSamples).WithOne(s => s.Channel).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Sample>().HasMany(s => s.ExternalImpacts).WithOne(i => i.Sample).OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ReportDB;Username=postgres;Password=admin");
    }
}
