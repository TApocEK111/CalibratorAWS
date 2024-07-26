using System.Text.Json.Serialization;

namespace Calibrator.Domain.Model.Report;

public class Coefficients
{
    public Guid Id { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    [JsonIgnore]
    public Guid SensorChannelId { get; set; }
}
