namespace Calibrator.Domain.Model.Report;

public class Sample
{
    public double ReferenceValue { get; set; }
    public DateTime MeasurementTime { get; set; }
    public double? PhisicalQuantityValue { get; set; }
    public double Paremeter { get; set; }
    public List<ExternalImpact> ExternalImpacts { get; set; }
}
