using Calibrator.Domain.Model.Setpoint;
using Calibrator.Infrastructure.Data;
using Calibrator.Infrastructure.Repository;
using Calibrator.Domain.Model.Report;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace Calibrator.API;

public class ExperimentManager
{
    public CalibrationStatus Status { get; set; } = CalibrationStatus.Inactive;
    private Setpoint _setpoint;
    private SetpointRepository _setpointRepository;
    private ReportRepository _reportRepository;
    public string ActuatorId { get; set; }
    public string SensorId { get; set; }
    private HttpClient _httpClient = new HttpClient();
    private readonly string _actuatorsUri = "https://localhost:7272/api/actuators/";
    private readonly string _sensorsUri = "http://localhost:7000/api/sensors/";
    public Report ResultReport { get; set; }
    private Exposure _previous { get; set; } = new Exposure() { Value = 0 };
    public Task CalibrationTask { get; set; }

    public enum CalibrationStatus
    {
        Inactive,
        GettingInitialSensorData,
        Calibrating,
        GettingCalibratedSensorData,
        AddingToDb,
        Finished
    }
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

    public async Task CalibrationAsync()
    {
        if (_setpoint == null)
            throw new ArgumentNullException("Setpoint is null");

        var calibrator = new Domain.Model.Calibrator.Calibrator();
        Status = CalibrationStatus.GettingInitialSensorData;
        ResultReport.Sensors[0].Channels[0].Samples = await GetSamplesAsync();
        Status = CalibrationStatus.Calibrating;
        ResultReport.Sensors[0].Channels[0].DefineSamplesDirections();
        ResultReport.Sensors[0].Channels[0].CalculateAverageSamples();
        calibrator.CalculatePhysicalQuantitylValues(ResultReport);
        await PostSensorConfigAsync(ResultReport.Sensors[0].Channels[0].Coefficients);

        Status = CalibrationStatus.GettingCalibratedSensorData;
        ResultReport.Sensors[0].Channels[0].Samples = await GetSamplesAsync();
        foreach (var sample in ResultReport.Sensors[0].Channels[0].Samples)
        {
            if (!IsInBounds(sample.ReferenceValue, sample.PhysicalQuantity, 0.1))
            {
                throw new ArgumentOutOfRangeException("Error is too big.");
            }
        }
        ResultReport.Sensors[0].Channels[0].DefineSamplesDirections();
        ResultReport.Sensors[0].Channels[0].CalculateAverageSamples();
        ResultReport.Sensors[0].Channels[0].DefineMaxError();
        //calibrator.CalculatePhysicalQuantitylValues(ResultReport);
        Status = CalibrationStatus.AddingToDb;
        await _reportRepository.AddAsync(ResultReport);
        Status = CalibrationStatus.Finished;
    }

    public async Task<List<Sample>> GetSamplesAsync()
    {
        var samples = new List<Sample>(_setpoint.Exposures.Count);
        foreach (var exposure in _setpoint.Exposures)
        {
            await PostExposureAsync(exposure);
            var actuator = await GetCurrentActuatorAsync();
            while (!actuator.isOnTarget)
            {
                await Task.Delay(250);
                actuator = await GetCurrentActuatorAsync();
            }

            var sample = await GetSensorSampleAsync();
            sample.ReferenceValue = exposure.Value;
            samples.Add(sample);
        }

        return samples;
    }

    public async Task<ActuatorDTO> GetCurrentActuatorAsync()
    {
        var response = await _httpClient.GetAsync(_actuatorsUri + ActuatorId) ?? throw new ArgumentNullException("No such actuator.");
        var responseStr = await response.Content.ReadAsStringAsync();
        var actuator = JsonSerializer.Deserialize<ActuatorDTO>(responseStr);

        return actuator;

    }

    public async Task<ActuatorDTO> PostExposureAsync(Exposure exposure)
    {
        var payload = new PostExposureDTO();
        payload.currentQuantity.value = _previous.Value;
        payload.targetQuantity.value = exposure.Value;
        payload.exposures.Add(new ExposureDTO { value = exposure.Value, duration = exposure.Duration, speed = exposure.Speed });

        var response = await _httpClient.PostAsJsonAsync(_actuatorsUri + ActuatorId, payload);
        _previous = exposure;
        return JsonSerializer.Deserialize<ActuatorDTO>(await response.Content.ReadAsStringAsync());
    }

    public async Task<bool> PostSensorConfigAsync(Coefficients coef)
    {
        var payload = new PostSensorConfigDTO();
        payload.approximateCoefficients = [coef.C, coef.B, coef.A];

        var response = await _httpClient.PostAsJsonAsync(_sensorsUri + SensorId + "/config", payload);
        return response.IsSuccessStatusCode;
    }   

    public async Task<Sample> GetSensorSampleAsync()

    {
        Sample sample = new Sample();
        var sensorResponse = await _httpClient.GetAsync(_sensorsUri + SensorId) ?? throw new ArgumentNullException("No such sensor.");
        SensorDTO sensor = JsonSerializer.Deserialize<SensorDTO>(await sensorResponse.Content.ReadAsStringAsync());
        sample.Parameter = sensor.parameter;
        sample.PhysicalQuantity = sensor.approximatedValue;

        return sample;
    }

    private bool IsInBounds(double expected, double arg, double error)
    {
        if (arg * expected < 0)
            return false;
        return Math.Abs(arg) >= Math.Abs(expected - expected * error) && Math.Abs(arg) <= Math.Abs(expected + expected * error);
    }

    public async Task SetSetpointAsync(Guid id) => _setpoint = await _setpointRepository.GetByIdAsync(id);

    public async Task SetSetpointAsync(string name) => _setpoint = await _setpointRepository.GetByNameAsync(name);

    public void SetSetpoint(Setpoint setpoint) => _setpoint = setpoint;

    public class SensorDTO
    {
        public CurrentDTO current { get; set; }
        public double parameter { get; set; }
        public double approximatedValue { get; set; }
    }
    public struct CurrentDTO
    {
        public string id { get; set; }
        public double value { get; set; }
        public string unit { get; set; } 
    }

    public class ActuatorDTO
    {
        public PhysicalQuantityDTO current { get; set; }
        public PhysicalQuantityDTO target { get; set; }
        public bool isOnTarget { get; set; }
        public List<PhysicalExposureDTO> exposures { get; set; }
        public List<ExternalFactorDTO> externalFactors { get; set; }

    }
    public class PhysicalQuantityDTO
    {
        public double value { get; set; }
        public string unit { get; set; }
    }
    public class PhysicalExposureDTO
    {
        double value { get; set; }
        double duration { get; set; }
        double speed { get; set; }
    }
    public class ExternalFactorDTO
    {
        string name { get; set; }
    }

    public class PostSensorConfigDTO
    {
        public double[] staticFunctionCoefficients { get; set; } = [5, 2, 0.05, 0.002];
        public double[] approximateCoefficients { get; set; }
    }

    public class PostExposureDTO
    {
        public CurrentQuantityDTO currentQuantity { get; set; } = new();
        public TargetQuantityDTO targetQuantity { get; set; } = new();
        public List<ExposureDTO> exposures { get; set; } = new List<ExposureDTO>();
    }
    public class CurrentQuantityDTO
    {
        public double value { get; set; }
        public string unit { get; set; } = "string";
    }
    public class TargetQuantityDTO
    {
        public double value { get; set; }
        public string unit { get; set; } = "string";
    }
    public class ExposureDTO
    {
        public double value { get; set; }
        public double duration { get; set; }
        public double speed { get; set; }
    }
}
