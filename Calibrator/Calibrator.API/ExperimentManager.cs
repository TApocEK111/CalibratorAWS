using Calibrator.Domain.Model.Setpoint;
using Calibrator.Infrastructure.Data;
using Calibrator.Infrastructure.Repository;

namespace Calibrator.API;

public class ExperimentManager
{
    private Setpoint _setpoint;
    private SetpointRepository _setpointRepository;
    private ReportRepository _reportRepository;

    public ExperimentManager(Context context)
    {
        _setpointRepository = new SetpointRepository(context);
        _reportRepository = new ReportRepository(context);
    }

    public ExperimentManager(SetpointRepository setRepo, ReportRepository repRepo)
    {
        _setpointRepository = setRepo;
        _reportRepository = repRepo;
    }

    public void Start()
    {

    }

    public async Task SetSetpoint(Guid id) => _setpoint = await _setpointRepository.GetByIdAsync(id);

    public async Task SetSetpoint(string name) => _setpoint = await _setpointRepository.GetByNameAsync(name);

    public void SetSetpoint(Setpoint setpoint) => _setpoint = setpoint;
}
