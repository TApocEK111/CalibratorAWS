using System.Text.Json.Serialization;

namespace Calibrator.Domain.Model.Report;

public class AverageSample
{
    public Guid Id { get; set; }
    public double ReferenceValue { get; set; }
    public double Parameter { get; set; }
    public double PhysicalQuantity { get; set; }

    [JsonIgnore]
    public SensorChannel Channel { get; set; }
}
