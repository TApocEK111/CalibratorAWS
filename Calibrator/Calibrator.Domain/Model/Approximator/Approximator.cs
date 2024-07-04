using Calibrator.Domain.Model.Report;

namespace Calibrator.Domain.Model.Approximator;

public static class Approximator
{
    static double[] coefficients;

    public static void CalculateCoefficients(SensorChannel channel) => coefficients = Matrix.GetCoeffitients(channel);

    public static double Aproximate(double parameter)
    {
        return coefficients[0] * parameter * parameter + coefficients[1] * parameter + coefficients[2];
    }

    public static void CalculateResults(SensorChannel channel)
    {
        var results = new Dictionary<Entry, double>();
        foreach (var parameter in channel.Parameters)
        {
            results.Add(parameter.Key, Aproximate(parameter.Value));
        }
        channel.Results = results;
    }
}
