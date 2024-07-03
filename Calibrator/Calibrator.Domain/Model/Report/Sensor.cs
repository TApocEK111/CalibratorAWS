namespace Calibrator.Domain.Model.Report;

public class Sensor
{
    public int Id { get; private set; }
    public string? Type { get; private set; } = null;
    public string? SerialNumber { get; private set; } = null;
    public string? SoftwareVersion { get; private set; } = null;
    public DateTime? ManufactureDate { get; private set; } = null;
    public int? EffectiveRange { get; private set; } = null;

}
