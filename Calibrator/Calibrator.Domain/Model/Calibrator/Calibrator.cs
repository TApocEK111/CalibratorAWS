using Calibrator.Domain.Model.Report;

namespace Calibrator.Domain.Model.Calibrator;

public class Calibrator
{
    private double[] coefficients;

    private void ApproximateCoefficients(SensorChannel channel) => coefficients = LeastSquareMethod.GetCoeffitients(channel);
    private double GradFunction(double parameter) => coefficients[0] * parameter * parameter + coefficients[1] * parameter + coefficients[2];
    private void CalculatePhysicalQuantity(SensorChannel channel)
    {
        ApproximateCoefficients(channel);
        foreach (var sample in channel.Samples)
        {
            sample.PhysicalQuantity = GradFunction(sample.Parameter);
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
