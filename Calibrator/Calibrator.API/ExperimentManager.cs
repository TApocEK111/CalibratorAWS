﻿using Calibrator.Domain.Model.Setpoint;
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
        if (_setpoint == null)
            throw new ArgumentNullException("Setpoint is null");

        var timer = new System.Timers.Timer();
    }

    public async Task PostSetpointDataAsync(Setpoint setpoint)
    {
        await _httpClient.PostAsync(_actuatorsUri + ActuatorId, new StringContent($"" +
            $"{{\r\n  " +
                $"\"currentQuantity\": {{\r\n    " +
                    $"\"value\": 0,\r\n    " +
                    $"\"unit\": \"string\"\r\n  " +
                $"}},\r\n  " +
                $"\"targetQuantity\": {{\r\n    " +
                    $"\"value\": {setpoint.Exposures[setpoint.Exposures.Count - 1].Value},\r\n    " +
                    $"\"unit\": \"string\"\r\n  " +
                $"}},\r\n  " +
                $"\"exposures\": [\r\n    " +
                    ExposuresToJsonList(setpoint) +
                $"]\r\n}}"));
    }

    private string ExposuresToJsonList(Setpoint setpoint)
    {
        var builder = new StringBuilder();
        int i = 0;
        foreach (var exposure in setpoint.Exposures)
        {
            builder.Append($"{{\r\n      \"value\": {exposure.Value},\r\n      \"duration\": {exposure.Duration},\r\n      \"speed\": {exposure.Speed}\r\n    }}");
            if (i < setpoint.Exposures.Count - 1)
                builder.Append(",\r\n  ");
            else 
                builder.Append("\r\n  ");
            i++;
        }

        return builder.ToString();
    }

    public async Task<Sample> GetSensorDataAsync()
    {
        Sample sample = new Sample();
        var sensorResponse = await _httpClient.GetAsync(_sensorsUri + SensorId) ?? throw new ArgumentNullException("No such sensor.");
        SensorDTO sensor = JsonSerializer.Deserialize<SensorDTO>(await sensorResponse.Content.ReadAsStringAsync());
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