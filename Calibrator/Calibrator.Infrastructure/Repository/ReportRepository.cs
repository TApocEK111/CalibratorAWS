using Calibrator.Domain.Model.Report;
using Calibrator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calibrator.Infrastructure.Repository;

public class ReportRepository
{
    private readonly Context _context;

    public Context UnitOfWork
    {
        get { return _context; }
    }

    public ReportRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Report>> GetAllAsync()
    {
        var reports = await _context.Reports
            .Include(r => r.Sensors)
            .ThenInclude(s => s.Channels)
            .ThenInclude(c => c.AvgSamples)
            .Include(r => r.Sensors)
            .ThenInclude(s => s.Channels)
            .ThenInclude(c => c.Samples)
            .ThenInclude(s => s.ExternalImpacts)
            .OrderBy(r => r.Date).ToListAsync();
        
        foreach (var report in reports)
        {
            foreach (var sensor in report.Sensors)
            {
                sensor.Channels.OrderBy(c => c.Number);
                foreach (var channel in sensor.Channels)
                {
                    channel.Samples.OrderBy(s => s.MeasurementTime);
                    channel.AvgSamples.OrderBy(s => s.ReferenceValue);
                }
            }
        }

        return reports;
    }

    public async Task<Report> GetByIdAsync(Guid id)
    {
        var result = (from report in _context.Reports
                        .Include(r => r.Sensors)
                        .ThenInclude(s => s.Channels)
                        .ThenInclude(c => c.AvgSamples)
                        .Include(r => r.Sensors)
                        .ThenInclude(s => s.Channels)
                        .ThenInclude(c => c.Samples)
                        .ThenInclude(s => s.ExternalImpacts)
                      where report.Id == id
                      select report).FirstOrDefaultAsync();


        return await result ?? throw new NullReferenceException("No such report");
    }

    public async Task<Report> AddAsync(Report report)
    {
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task UpdateAsync(Report report)
    {
        Report existReport = await _context.Reports.FindAsync(report.Id) ?? throw new NullReferenceException("No such report");
        _context.Entry(existReport).CurrentValues.SetValues(report);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Report report = await _context.Reports.FindAsync(id) ?? throw new NullReferenceException("No such report");
        _context.Remove(report);
        await _context.SaveChangesAsync();
    }

    public void ChangeTrackerClear()
    {
        _context.ChangeTracker.Clear();
    }
}
