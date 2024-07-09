namespace Calibrator.Domain.Model.Report;

public class Sample
{
    public Guid Id { get; set; }
    public double ReferenceValue { get; set; }
    public DateTime MeasurementTime { get; set; } = DateTime.Now;
    public double PhysicalQuantity { get; set; }
    public double Parameter { get; set; }
    public double Error { get { return Math.Abs(ReferenceValue - this.PhysicalQuantity); } }
    public List<ExternalImpact> ExternalImpacts { get; set; } = new List<ExternalImpact>();
    public Direction Direction { get; set; } = Direction.Undefined;
}
public enum Direction
{
    Undefined,
    Forward,
    Reverse
}