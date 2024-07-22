using Calibrator.Infrastructure.Data;

namespace Calibrator.API;

public class ManagersContainer
{
    private Context _context;
    private Dictionary<Guid, ExperimentManager> _managers = new Dictionary<Guid, ExperimentManager>();

    public ManagersContainer(Context context)
    {
        _context = context;
    }
    public ExperimentManager GetManagerById(Guid id)
    {
        try
        {
            return _managers[id];
        }
        catch (KeyNotFoundException)
        {
            _managers.Add(id, new ExperimentManager(_context));
        }
        return _managers[id];
    }
}
