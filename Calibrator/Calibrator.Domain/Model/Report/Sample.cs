namespace Calibrator.Domain.Model.Report;

public class Sample
{
    public double ReferenceValue { get; set; }
    public DateTime MeasurementTime { get; set; }
    public double? ResultantPhysicalQuantity { get; set; }  //над названием стоит еще подумать. это точно...
    public double Parameter { get; set; }
    public double Error { get; set; }
    public List<ExternalImpact> ExternalImpacts { get; set; }
}
