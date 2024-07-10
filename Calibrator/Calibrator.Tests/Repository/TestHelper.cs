using Calibrator.Infrastructure.Data;
using Calibrator.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Calibrator.Tests.Repository;

public class TestHelper
{
    private readonly Context _context;
    public Report TestReport = new Report
    {
        Sensors = new List<Sensor>
            {
                new Sensor
                {
                    Channels = new List<SensorChannel>()
                    {
                        new SensorChannel()
                        {
                            Samples = new List<Sample>()
                            {
                                new Sample() { ReferenceValue = 50, Parameter = 51 },
                                new Sample() { ReferenceValue = 50, Parameter = 48 },
                                new Sample() { ReferenceValue = 60, Parameter = 62 },
                                new Sample() { ReferenceValue = 60, Parameter = 59 },
                                new Sample() { ReferenceValue = 70, Parameter = 70 },
                                new Sample() { ReferenceValue = 50, Parameter = 51 }
                            }
                        }
                    }
                }
            }
    };

    public ReportRepository ReportRepository
    {
        get
        {
            return new ReportRepository(_context);
        }
    }

    public TestHelper()
    {
        var contextOptions = new DbContextOptionsBuilder<Context>()
            .UseNpgsql("Host=localhost;Port=5432;Database=TestReportDB;Username=postgres;Password=admin")
            .Options;

        _context = new Context(contextOptions);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var report = new Report
        {
            Date = DateTime.MinValue.ToUniversalTime(),
            Sensors = new List<Sensor>
            {
                new Sensor
                {
                    Channels = new List<SensorChannel>()
                    {
                        new SensorChannel()
                        {
                            Samples = new List<Sample>()
                            {
                                new Sample() { ReferenceValue = -10, Parameter = -1 },
                                new Sample() { ReferenceValue = -10, Parameter = -0.9 },
                                new Sample() { ReferenceValue = -20, Parameter = -2 },
                                new Sample() { ReferenceValue = -30, Parameter = -2.9 },
                                new Sample() { ReferenceValue = -20, Parameter = -2.1 },
                                new Sample() { ReferenceValue = -20, Parameter = -1.9 },
                                new Sample() { ReferenceValue = -20, Parameter = -2 },
                                new Sample() { ReferenceValue = -10, Parameter = -1.1 }
                            }
                        }
                    }
                }
            }
        };

        report.Sensors[0].Channels[0].CalculateAverageSamples();
        TestReport.Sensors[0].Channels[0].CalculateAverageSamples();

        var calibrator = new Domain.Model.Calibrator.Calibrator();
        calibrator.CalculatePhysicalQuantitylValues(report);

        _context.Add(report);
        _context.SaveChanges();

        _context.ChangeTracker.Clear();
    }
}
