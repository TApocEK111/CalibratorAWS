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
    private Setpoint _setpoint;
    private SetpointRepository _setpointRepository;
    private ReportRepository _reportRepository;
    public string ActuatorId { get; set; }
    public string SensorId { get; set; }
    private HttpClient _httpClient = new HttpClient();
    private readonly string _actuatorsUri = "https://actuatorsim.socketsomeone.me/api/actuators/";
    private readonly string _sensorsUri = "https://sensorsim.socketsomeone.me/api/sensors/";
    public Report ResultReport { get; set; }

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
        ResultReport.Sensors[0].Channels[0].Samples = await GetSamplesAsync();
        ResultReport.Sensors[0].Channels[0].DefineSamplesDirections();
        ResultReport.Sensors[0].Channels[0].CalculateAverageSamples();
        calibrator.CalculatePhysicalQuantitylValues(ResultReport);
        await PostSensorConfigAsync(ResultReport.Sensors[0].Channels[0].Coefficients);

        ResultReport.Sensors[0].Channels[0].Samples = await GetSamplesAsync();
        foreach (var sample in ResultReport.Sensors[0].Channels[0].Samples)
        {
            if (!IsInBounds(sample.ReferenceValue, sample.PhysicalQuantity, 0.05))
            {
                throw new ArgumentOutOfRangeException("Error is too big.");
            }
        }

        ResultReport.Sensors[0].Channels[0].DefineSamplesDirections();
        ResultReport.Sensors[0].Channels[0].CalculateAverageSamples();
        ResultReport.Sensors[0].Channels[0].DefineMaxError();
        calibrator.CalculatePhysicalQuantitylValues(ResultReport);
        await _reportRepository.AddAsync(ResultReport);
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
        var response = await _httpClient.PostAsync(_actuatorsUri + ActuatorId, new StringContent($"{{  \"currentQuantity\": {{    \"value\": 0,    \"unit\": \"string\"  }},  \"targetQuantity\": {{    \"value\": {exposure.Value},    \"unit\": \"string\"  }},  \"exposures\": [    {{      \"value\": {exposure.Value},      \"duration\": {exposure.Duration},      \"speed\": {exposure.Speed}    }}  ]}}", Encoding.UTF8, "application/json"));
        return JsonSerializer.Deserialize<ActuatorDTO>(await response.Content.ReadAsStringAsync());
    }

    public async Task<bool> PostSensorConfigAsync(Coefficients coef)
    {
        var response = await _httpClient.PostAsync(_sensorsUri + SensorId + "/config", new StringContent($"{{\r\n  \"staticFunctionConfig\": {{\r\n    \"type\": \"string\",\r\n    \"coefficients\": [\r\n      {coef.C},\r\n      {coef.B},\r\n      {coef.A}\r\n    ]\r\n }}\r\n}}"));
        return response.IsSuccessStatusCode;
    }   

    public async Task<Sample> GetSensorSampleAsync()

    {
        Sample sample = new Sample();
        var sensorResponse = await _httpClient.GetAsync(_sensorsUri + SensorId) ?? throw new ArgumentNullException("No such sensor.");
        SensorDTO sensor = JsonSerializer.Deserialize<SensorDTO>(await sensorResponse.Content.ReadAsStringAsync());
        sample.Parameter = sensor.parameter;
        sample.PhysicalQuantity = sensor.current.value;

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

}
