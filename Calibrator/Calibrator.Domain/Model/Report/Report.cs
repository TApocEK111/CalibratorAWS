namespace Calibrator.Domain.Model.Report;

public class Report
{
    public string Operator { get; set; }
    public DateTime Date { get; set; }
    public List<SensorChannel> Channels { get; set; }
    public List<Experiment> Experiments { get; set; }
    public Dictionary<string, List<double>> EnvironmentProperties { get; set; }

}
