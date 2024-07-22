using Calibrator.Domain.Model.Report;
using Calibrator.Domain.Model.Setpoint;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Calibrator.API.Controllers
{
    [Route("api/calibrator")]
    [ApiController]
    public class CalibratorController : Controller
    {
        private ManagersContainer _container;

        public CalibratorController(ManagersContainer container)
        {
            _container = container;
        }

        [HttpPost]
        public IActionResult Calibration([FromBody] StartCalibrationRequestModel startCalibrationRequestModel)
        {
            var report = new Report();
            var manager = _container.GetManagerById(new Guid(startCalibrationRequestModel.userInfo.Id));

            report.Sensors.Add(new Sensor()
            {
                Type = startCalibrationRequestModel.sensorInfo.Type,
                SerialNumber = startCalibrationRequestModel.sensorInfo.SerialNumber,
                SoftwareVersion = startCalibrationRequestModel.sensorInfo.SerialNumber,
                ManufactureDate = DateTime.Parse(startCalibrationRequestModel.sensorInfo.ManufactureDate),
                EffectiveRangeMin = startCalibrationRequestModel.sensorInfo.EffectiveRangeMin,
                EffectiveRangeMax = startCalibrationRequestModel.sensorInfo.EffectiveRangeMax
            });
            report.Sensors[0].Channels.Add(new SensorChannel()
            {
                Number = 1,
                PhisicalQuantity = (PhysicalQuantity)startCalibrationRequestModel.PhysicalQuantityType
            });
            manager.ResultReport = report;
            manager.SetSetpoint(new Setpoint()
            {
                Id = new Guid(startCalibrationRequestModel.setpointInfo.Id),
                Name = startCalibrationRequestModel.setpointInfo.Name,
                Exposures = startCalibrationRequestModel.setpointInfo.Exposures
            });
            var calibration = manager.CalibrationAsync();
            Task.WaitAll(calibration);

            return Ok(report);
        }



        public class StartCalibrationRequestModel
        {
            public UserInfoModel userInfo { get; set; }
            public SensorInfoModel sensorInfo { get; set; }
            public SetpointInfoModel setpointInfo { get; set; }
            public int PhysicalQuantityType { get; set; }
        }
        public class SetpointInfoModel
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public List<Exposure> Exposures { get; set; }
        }
        public class UserInfoModel
        {
            public string Name { get; set; } = string.Empty;
            public string Id { get; set; }
        }
        public class SensorInfoModel
        {
            public string Type {  get; set; } = string.Empty;
            public string SerialNumber { get; set; } = string.Empty;
            public string SoftwareVersion { get; set; } = string.Empty;
            public string ManufactureDate { get; set; } = string.Empty;
            public double EffectiveRangeMin { get; set; }
            public double EffectiveRangeMax { get; set; }
        }
    }
}
