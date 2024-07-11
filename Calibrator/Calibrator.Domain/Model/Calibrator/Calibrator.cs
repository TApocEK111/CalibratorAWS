using Calibrator.Domain.Model.Report;

namespace Calibrator.Domain.Model.Calibrator;

public class Calibrator
{
    private void ApproximateCoefficients(SensorChannel channel)
    {
        var coef = LeastSquareMethod.GetCoeffitients(channel);
        channel.Coefficients = new Coefficients() { A = coef[0], B = coef[1], C = coef[2] };
    }
    
    private void CalculatePhysicalQuantity(SensorChannel channel)
    {
        ApproximateCoefficients(channel);
        foreach (var sample in channel.Samples)
        {
            sample.PhysicalQuantity = channel.Coefficients.A * sample.Parameter * sample.Parameter + channel.Coefficients.B * sample.Parameter + channel.Coefficients.C;
        }
    }

    public void CalculatePhysicalQuantitylValues(Report.Report report)
    {
        foreach (var sensor in report.Sensors)
        {
            foreach (var channel in sensor.Channels)
            {
                CalculatePhysicalQuantity(channel);
            }
        }
    }
}
