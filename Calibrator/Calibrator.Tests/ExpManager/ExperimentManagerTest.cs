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
        List<Exposure> exposures = new List<Exposure> { new Exposure { Speed = 1, Value = 5, Number = 1 }, new Exposure { Speed = 1, Value = 11, Number = 2 }, new Exposure { Speed = 1, Value = 16, Number = 3 }, new Exposure { Speed = 1, Value = 20, Number = 4 } };
        ExperimentManager manager = new ExperimentManager(new Context(new DbContextOptionsBuilder<Context>()
            .UseNpgsql("Host=localhost;Port=5432;Database=TestReportDB;Username=postgres;Password=admin")
            .Options));
        manager.ActuatorId = "2";
        manager.SensorId = "2";

        //await manager.PostSensorConfigAsync(new Coefficients { A = 1, B = 2, C = 3 });
        Setpoint setpoint = new Setpoint { Exposures = exposures, Id = Guid.NewGuid(), Name = "123" };
        manager.ResultReport = new Report
        {
            Sensors = new List<Sensor>
    {
        new Sensor()
            {
                Channels = new List<SensorChannel>
                {
                    new SensorChannel()
                }
            }
    }
        };
        manager.SetSetpoint(setpoint);
        await manager.CalibrationAsync();
        HttpClient client = new HttpClient();
        var response = await client.GetAsync("https://sensorsim.socketsomeone.me/api/sensors/" + manager.SensorId + "config");
        Assert.NotNull(response);
        //var actuator = await manager.PostExposureAsync(exposure);
        //Assert.True(actuator.isOnTarget == false);

        //await Task.Delay(2000);
        //Assert.True((await manager.GetCurrentActuatorAsync()).isOnTarget == false);

        //await Task.Delay(6000);
        //var actuator2 = await manager.GetCurrentActuatorAsync();
        //Assert.True(actuator2.isOnTarget == true);


        //Sample sample = await manager.GetSensorSampleAsync();


    }
}
