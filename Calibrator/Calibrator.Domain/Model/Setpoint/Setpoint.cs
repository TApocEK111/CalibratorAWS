namespace Calibrator.Domain.Model.Setpoint;

public class Setpoint
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public List<Exposure> Exposures { get; set; } = [];
}
