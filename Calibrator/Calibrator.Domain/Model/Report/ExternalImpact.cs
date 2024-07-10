namespace Calibrator.Domain.Model.Report;

public class ExternalImpact
{
    public Guid Id { get; set; }
    public double Value { get; set; }
    public PhysicalQuantity PhisicalQuantity { get; set; } = PhysicalQuantity.Udefined;

    public Sample Sample { get; set; }
}
