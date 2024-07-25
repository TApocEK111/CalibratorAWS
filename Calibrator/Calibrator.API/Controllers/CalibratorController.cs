using Calibrator.Domain.Model.Report;
using Calibrator.Domain.Model.Setpoint;
using Microsoft.AspNetCore.Mvc;
using Calibrator.Infrastructure.Repository;

namespace Calibrator.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalibratorController : Controller
    {
        private ManagersContainer _container;
        private SetpointRepository _setRepo;
        private ReportRepository _reportRepo;

        public CalibratorController(ManagersContainer container, SetpointRepository setRepo, ReportRepository reportRepo)
        {
            _container = container;
            _setRepo = setRepo;
            _reportRepo = reportRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Calibration([FromBody] StartCalibrationRequestModel startCalibrationRequestModel)
        {
            var report = new Report();
            var manager = _container.GetManagerById(new Guid(startCalibrationRequestModel.userInfo.Id));
            manager.SensorId = startCalibrationRequestModel.sensorId;
            manager.ActuatorId = startCalibrationRequestModel.actuatorId;

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
            var setpoint = await _setRepo.GetByIdAsync(startCalibrationRequestModel.setpointId);
            manager.SetSetpoint(setpoint);
            report.SetpointId = setpoint.Id;
            manager.CalibrationTask = manager.CalibrationAsync();

            return Ok(report.Id);
        }

        [HttpPost]
        public async Task<ActionResult<Setpoint>> SetSetpoint(Setpoint setpoint)
        {
            await _setRepo.AddAsync(setpoint);
            return Ok();
        }

        [HttpGet("{setpointId}/setpoint")]
        public async Task<ActionResult<Setpoint>> GetSetpoint(Guid setpointId)
        {
            var setpoint = await _setRepo.GetByIdAsync(setpointId);
            return setpoint;
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReport(Guid reportId)
        {
            Report report = await _reportRepo.GetByIdAsync(reportId);
            return Ok(report);
        }

        [HttpGet("{userId}/status")]
        public IActionResult GetCurrentStatus(Guid userId)
        {
            return Ok(_container.GetManagerById(userId).Status);
        }

        public class StartCalibrationRequestModel
        {
            public UserInfoModel userInfo { get; set; }
            public SensorInfoModel sensorInfo { get; set; }
            public Guid setpointId { get; set; }
            public string sensorId { get; set; }
            public string actuatorId { get; set; }
            public int PhysicalQuantityType { get; set; }
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
