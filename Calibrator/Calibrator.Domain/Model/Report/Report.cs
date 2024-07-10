namespace Calibrator.Domain.Model.Report;

public class Report
{
    public Guid Id { get; set; }
    public string Operator { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
    public List<Sensor> Sensors { get; set; } = new List<Sensor>();
}

public enum PhysicalQuantity
{
    Udefined,
    Temperature,
    Mass,
    Force,
    Preassure
}