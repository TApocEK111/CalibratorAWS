namespace Calibrator.Domain.Model.Report;

public class Sensor
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string SoftwareVersion { get; set; } = string.Empty;
    public DateTime? ManufactureDate { get; set; } //must be in UTC
    public double EffectiveRangeMin {  get; set; }
    public double EffectiveRangeMax { get; set; }

    public List<SensorChannel> Channels { get; set; } = new List<SensorChannel>();

    public Guid ReportId { get; set; }
    public Report Report { get; set; }
}
