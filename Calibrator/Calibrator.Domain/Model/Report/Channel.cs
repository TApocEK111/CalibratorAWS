namespace Calibrator.Domain.Model.Report;

public class Channel
{
    public Sensor Sensor { get; set; }
    public Dictionary<Experiment, double> Parameters { get; set; }
    public Dictionary<Experiment, double> Results { get; set; }
}
