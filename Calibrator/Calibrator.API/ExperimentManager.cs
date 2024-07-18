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
    public string ActuatorId { get; private set; }
    public string SensorId { get; private set; }
    private HttpClient _httpClient = new HttpClient();
    private readonly string _actuatorsUri = "https://actuatorsim.socketsomeone.me/api/actuators/";
    private readonly string _sensorsUri = "https://sensorsim.socketsomeone.me/api/sensors/";
    public bool IsActuatorReady { get; set; } = false;
    public Report ResultReport {  get; set; } = new Report
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

        ResultReport.Sensors[0].Channels[0].Samples = await GetSamplesAsync();
        ResultReport.Sensors[0].Channels[0].DefineSamplesDirections();
        ResultReport.Sensors[0].Channels[0].CalculateAverageSamples();
        var calibrator = new Domain.Model.Calibrator.Calibrator();
        calibrator.CalculatePhysicalQuantitylValues(ResultReport);
        await PostSensorConfigAsync(ResultReport.Sensors[0].Channels[0].Coefficients);
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
            samples.Add(sample);
        }

        return samples;
    }

    private async Task<ActuatorDTO> GetCurrentActuatorAsync()
    {
        var response = await _httpClient.GetAsync(_actuatorsUri + ActuatorId) ?? throw new ArgumentNullException("No such actuator.");
        var responseStr = await response.Content.ReadAsStringAsync();
        var actuator = JsonSerializer.Deserialize<ActuatorDTO>(responseStr);

        return actuator;

    }

    public async Task PostExposureAsync(Exposure exposure)
    {
        await _httpClient.PostAsync(_actuatorsUri + ActuatorId, new StringContent($"" +
            $"{{\r\n  " +
                $"\"currentQuantity\": {{\r\n    " +
                    $"\"value\": 0,\r\n    " +
                    $"\"unit\": \"string\"\r\n  " +
                $"}},\r\n  " +
                $"\"targetQuantity\": {{\r\n    " +
                    $"\"value\": {exposure.Value},\r\n    " +
                    $"\"unit\": \"string\"\r\n  " +
                $"}},\r\n  " +
                $"\"exposures\": [\r\n    " +
                $"{{\r\n      \"value\": {exposure.Value},\r\n      \"duration\": {exposure.Duration},\r\n      \"speed\": {exposure.Speed}\r\n    }}" +
                $"]\r\n}}"));
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
