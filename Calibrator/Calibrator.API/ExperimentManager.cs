using Calibrator.Domain.Model.Setpoint;
using Calibrator.Infrastructure.Data;
using Calibrator.Infrastructure.Repository;
using Calibrator.Domain.Model.Report;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Calibrator.API;

public class ExperimentManager
{
    private Setpoint _setpoint;
    private SetpointRepository _setpointRepository;
    private ReportRepository _reportRepository;
    public string ActuatorId { get; private set; }
    public string SensorId { get; private set; }
    private HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("https://actuatorsim.socketsomeone.me/api/actuators/") };
    public bool IsActuatorReady { get; set; } = false;

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

    public async Task Start()
    {
        var stopwatch = new System.Diagnostics.Stopwatch();
        foreach (var exposure in _setpoint.Exposures)
        {
            stopwatch.Stop();
            stopwatch.Reset();
            await PostSetpointDataAsync(exposure);
            while (!IsActuatorReady) { }
            stopwatch.Start();
            var sample = await GetSensorDataAsync();
            sample.ReferenceValue = exposure.Value;
        }
    }

    public async Task PostSetpointDataAsync(Exposure exposure)
    {
        await _httpClient.PostAsync(_httpClient.BaseAddress + ActuatorId, new StringContent($"{{\r\n  \"value\": {exposure.Value},\r\n  \"exposures\": [\r\n    {{\r\n      \"value\": {exposure.Value},\r\n      \"duration\": {exposure.Duration},\r\n      \"speed\": {exposure.Speed}\r\n    }}\r\n  ]\r\n}}"));
    }

    public async Task<Sample> GetSensorDataAsync()
    {
        Sample sample = new Sample();
        var sensorResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + SensorId);
        SensorDTO sensor = await sensorResponse.Content.ReadFromJsonAsync<SensorDTO>();
        sample.Parameter = sensor.Parameter;
        sample.PhysicalQuantity = sensor.Current.Value;

        return sample;
    }

    public async Task SetSetpoint(Guid id) => _setpoint = await _setpointRepository.GetByIdAsync(id);

    public async Task SetSetpoint(string name) => _setpoint = await _setpointRepository.GetByNameAsync(name);

    public void SetSetpoint(Setpoint setpoint) => _setpoint = setpoint;

    private class SensorDTO
    {
        internal CurrentDTO Current { get; set; }
        internal double Parameter { get; set; }
    }
    private struct CurrentDTO
    {
        internal string Id { get; set; }
        internal double Value { get; set; }
        internal string Unit { get; set; } 
    }
}
