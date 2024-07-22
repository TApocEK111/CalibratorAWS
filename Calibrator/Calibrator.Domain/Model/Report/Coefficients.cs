using System.Text.Json.Serialization;

namespace Calibrator.Domain.Model.Report;

public class Coefficients
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    [JsonIgnore]
    public SensorChannel Channel { get; set; }
}
