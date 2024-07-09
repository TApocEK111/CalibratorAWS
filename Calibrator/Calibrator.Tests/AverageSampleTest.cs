namespace Calibrator.Tests;

public class AverageSampleTest
{
    [Theory]
    [InlineData(new double[] { 1, 2, 3, 4, 5, 6 }, new double[] { 200.25, 400.4, 600.2, 800.5, 999, 1200.333333 }, new double[] { 1, 1, 1, 2, 2, 2, 2, 3, 4, 5, 6, 6, 6, 5, 4, 3, 3, 3, 3, 2, 1 }, new double[] { 201, 202, 198, 403, 401, 398, 400, 601, 800, 998, 1200, 1202, 1199, 1000, 801, 600, 598, 601, 601, 400, 200 })]
    [InlineData(new double[] { 50, 60, 70}, new double[] { 50, 60.5, 70 }, new double[] { 50, 50, 60, 60, 70, 50 }, new double[] { 51, 48, 62, 59, 70, 51 })]
    [InlineData(new double[] { -10, -20, -30}, new double[] { -1, -2, -2.9}, new double[] { -10, -10, -20, -30, -20, -20, -20, -10  }, new double[] { -1, -0.9, -2, -2.9, -2.1, -1.9, -2, -1.1})]

    public void AverageSamplesCorrect(double[] expectedRef, double[] expectedPar, double[] references, double[] parameters)
    {
        var channel = new SensorChannel() { Samples = new List<Sample>(parameters.Length) };
        for (int i = 0; i < parameters.Length; i++)
        {
            channel.Samples.Add(new Sample() { ReferenceValue = references[i], Parameter = parameters[i] });
        }

        int j = 0;
        foreach (var sample in channel.AvgSamples)
        {
            Assert.True(IsInBounds(sample.ReferenceValue, expectedRef[j], 0.05));
            Assert.True(IsInBounds(sample.Parameter, expectedPar[j], 0.05));
            j++;
        }
    }
    private bool IsInBounds(double expected, double arg, double error)
    {
        if (arg * expected < 0)
            return false;
        return Math.Abs(arg) >= Math.Abs(expected - expected * error) && Math.Abs(arg) <= Math.Abs(expected + expected * error);
    }
}
