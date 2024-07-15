using Calibrator.Domain.Model.Setpoint;
using Calibrator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calibrator.Infrastructure.Repository;

public class SetpointRepository
{
    private readonly Context _context;

    public Context UnitOfWork
    {
        get { return _context; }
    }
    
    public SetpointRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Setpoint>> GetAllAsync()
    {
        var result = await _context.Setpoints
            .Include(s => s.Exposures)
            .ToListAsync() ?? throw new NullReferenceException("No such preset.");

        foreach (var setpoint in result)
        {
            setpoint.Exposures = setpoint.Exposures.OrderBy(e => e.Number).ToList();
        }

        return result;
    }

    public async Task<Setpoint> GetByIdAsync(Guid id)
    {
        var result = await _context.Setpoints
            .Include(s => s.Exposures)
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync() ?? throw new NullReferenceException("No such preset.");

        result.Exposures = result.Exposures.OrderBy(e => e.Number).ToList();

        return result;
    }

    public async Task<Setpoint> AddAsync(Setpoint settings)
    {
        _context.Setpoints.Add(settings);
        await _context.SaveChangesAsync();
        return settings;
    }

    public async Task<Setpoint> GetByNameAsync(string name)
    {
        var result = await _context.Setpoints
            .Include(s => s.Exposures)
            .Where(s => s.Name == name)
            .FirstOrDefaultAsync() ?? throw new NullReferenceException("No such preset.");

        result.Exposures = result.Exposures.OrderBy(e => e.Number).ToList();

        return result;
    }
}
