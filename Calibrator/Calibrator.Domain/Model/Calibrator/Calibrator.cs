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

    private static void CalculateTargetPhisicalValues(SensorChannel channel)
    {
        CalculateCoefficients(channel);
        foreach (var samle in channel.Samples)
        {
            samle.TargetQuantityValue = Approximate(samle.Paremeter);
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
