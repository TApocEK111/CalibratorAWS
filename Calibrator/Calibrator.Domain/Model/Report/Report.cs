namespace Calibrator.Domain.Model.Report;

public class Report
{
    public string Operator { get; set; }
    public DateTime Date { get; set; }
    public List<Sensor> Sensors { get; set; }
}

public enum PhisicalQuantity
{
    Temperature,
    Mass,
    Force,
    Preassure
}