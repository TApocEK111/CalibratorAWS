using System.Text.Json.Serialization;

namespace Calibrator.Domain.Model.Report;

public class Sensor
{
    private DateTime _manufactureDate;

    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime ManufactureDate { get { return _manufactureDate; } set { _manufactureDate = value.ToUniversalTime(); } }
    public double EffectiveRangeMin {  get; set; }
    public double EffectiveRangeMax { get; set; }
    public List<SensorChannel> Channels { get; set; } = new List<SensorChannel>();
    
    [JsonIgnore]
    public Report Report { get; set; }
}
