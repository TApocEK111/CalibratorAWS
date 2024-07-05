using Calibrator.Domain.Model.Report;

namespace Calibrator.Domain.Model.Calibrator;

public class Calibrator
{
    private static double[] coefficients;

    private static void CalculateCoefficients(SensorChannel channel) => coefficients = LeastSquareMethod.GetCoeffitients(channel);

    private static double Approximate(double parameter)
    {
        return coefficients[0] * parameter * parameter + coefficients[1] * parameter + coefficients[2];
    }

    public static void CalculateTargetPhisicalValues(SensorChannel channel) //private
    {
        CalculateCoefficients(channel);
        foreach (var sample in channel.Samples)
        {
            sample.ResultantPhysicalQuantity = Approximate(sample.Parameter);
            sample.Error = Math.Abs(sample.ReferenceValue - (double)sample.ResultantPhysicalQuantity);
        }
    }

    public static void CalculateTargetPhisicalValues(Report.Report report)
    {
        foreach (var sensor in report.Sensors)
        {
            foreach (var channel in sensor.Channels)
            {
                CalculateTargetPhisicalValues(channel);
            }
        }
    }
}
