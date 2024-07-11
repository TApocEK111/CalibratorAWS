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
                    .ThenInclude(c => c.AverageSamples)
            .Include(r => r.Sensors)
                .ThenInclude(s => s.Channels)
                    .ThenInclude(c => c.Samples)
                        .ThenInclude(s => s.ExternalImpacts)
            .OrderBy(r => r.Date).ToListAsync();
        
        for (int i = 0; i < reports.Count; i++)
        {
            for (int j = 0; j < reports[i].Sensors.Count; j++)
            {
                reports[i].Sensors[j].Channels = reports[i].Sensors[j].Channels.OrderBy(c => c.Number).ToList();
                for (int k = 0; k < reports[i].Sensors[j].Channels.Count; k++)
                {
                    reports[i].Sensors[j].Channels[k].Samples = reports[i].Sensors[j].Channels[k].Samples.OrderBy(s => s.MeasurementTime).ToList();
                    reports[i].Sensors[j].Channels[k].AverageSamples = reports[i].Sensors[j].Channels[k].AverageSamples.OrderBy(s => s.ReferenceValue).ToList();
                }
            }
        }

        return reports;
    }

    public async Task<Report> GetByIdAsync(Guid id)
    {
        var result = await _context.Reports
            .Include(r => r.Sensors)
                .ThenInclude(s => s.Channels)
                    .ThenInclude(c => c.AverageSamples)
            .Include(r => r.Sensors)
                .ThenInclude(s => s.Channels)
                    .ThenInclude(c => c.Samples)
                        .ThenInclude(s => s.ExternalImpacts)
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        if (result == null)
            throw new NullReferenceException("No such report");

        for (int j = 0; j < result.Sensors.Count; j++)
        {
            result.Sensors[j].Channels = result.Sensors[j].Channels.OrderBy(c => c.Number).ToList();
            for (int k = 0; k < result.Sensors[j].Channels.Count; k++)
            {
                result.Sensors[j].Channels[k].Samples = result.Sensors[j].Channels[k].Samples.OrderBy(s => s.MeasurementTime).ToList();
                result.Sensors[j].Channels[k].AverageSamples = result.Sensors[j].Channels[k].AverageSamples.OrderBy(s => s.ReferenceValue).ToList();
            }
        }

        return result;
    }

    public async Task<Report> AddAsync(Report report)
    {
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    public async Task UpdateAsync(Report report)
    {
        var existReport = await GetByIdAsync(report.Id)/*_context.Reports.FindAsync(report.Id) ?? throw new NullReferenceException("No such report")*/;
        _context.Entry(existReport).CurrentValues.SetValues(report);

        var sensors = existReport.Sensors;
        foreach (var sensor in sensors)
        {
            _context.Entry(sensor).CurrentValues.SetValues(sensor);
            var channels = sensor.Channels;
            foreach (var channel in channels)
            {
                _context.Entry(channel).CurrentValues.SetValues(channel);
                var samples = channel.Samples;
                foreach (var sample in samples)
                {
                    _context.Entry(sample).CurrentValues.SetValues(sample);
                    var externalImpacts = sample.ExternalImpacts;
                    foreach (var externalImpact in externalImpacts)
                    {
                        _context.Entry(externalImpact).CurrentValues.SetValues(externalImpact);
                    }
                }
                var avgSamples = channel.AverageSamples;
                foreach (var avgSample in avgSamples)
                {
                    _context.Entry(avgSample).CurrentValues.SetValues(avgSample);
                }
            }
        }

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
