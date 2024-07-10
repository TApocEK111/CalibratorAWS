namespace Calibrator.Domain.Model.Report;

public class Sample
{
    private DateTime _measurementTime = DateTime.Now.ToUniversalTime();

    public Guid Id { get; set; }
    public double ReferenceValue { get; set; }
    public DateTime MeasurementTime { get { return _measurementTime; } set { _measurementTime = value.ToUniversalTime(); } }
    public double PhysicalQuantity { get; set; }
    public double Parameter { get; set; }
    public double Error { get { return Math.Abs(ReferenceValue - PhysicalQuantity); } }
    public List<ExternalImpact> ExternalImpacts { get; set; } = new List<ExternalImpact>();
    public Direction Direction { get; set; } = Direction.Undefined;

    public SensorChannel Channel { get; set; }
}
public enum Direction
{
    Undefined,
    Forward,
    Reverse
}