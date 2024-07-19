using Calibrator.API;
using Calibrator.Domain.Model.Setpoint;
using Calibrator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Calibrator.Tests.ExpManager;

public class ExperimentManagerTest
{
    [Fact]
    public async Task PostExposure()
    {
        Exposure exposure = new Exposure { Speed = 1, Value = 5 };
        ExperimentManager manager = new ExperimentManager(new Context(new DbContextOptionsBuilder<Context>()
            .UseNpgsql("Host=localhost;Port=5432;Database=TestReportDB;Username=postgres;Password=admin")
            .Options));
        manager.ActuatorId = "1";
        manager.SensorId = "1";

        
        var actuator = await manager.PostExposureAsync(exposure);
        Assert.True(actuator.isOnTarget == false);

        await Task.Delay(2000);
        Assert.True((await manager.GetCurrentActuatorAsync()).isOnTarget == false);

        await Task.Delay(6000);
        var actuator2 = await manager.GetCurrentActuatorAsync();
        Assert.True(actuator2.isOnTarget == true);

        Sample sample = await manager.GetSensorSampleAsync();
    }
}
