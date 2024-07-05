namespace Calibrator.Tests;

public class LSMTests
{
    [Theory]
    [InlineData(new double[] { -0.000064, 1.05, -9.2 }, new double[] { 200, 198, 300, 303, 400, 404, 500, 498, 600, 602 })]
    [InlineData(new double[] { 0.192926, -3.2684, 14.2907 }, new double[] { 10.1, 1.2, 12.6, 2.8, 14.8, 7.6, 16, 12.8, 17.5, 15.1 })]
    [InlineData(new double[] { 0, 1, 0 }, new double[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 })]
    [InlineData(new double[] { 0, 1, 1 }, new double[] { 1, 2, 2, 3, 3, 4, 4, 5, 5, 6 })]
    public void CalculatesCorrectCoefficients(double[] expected, double[] args)
    {
        SensorChannel channel = new SensorChannel()
        {
            Samples = new List<Sample>(args.Length / 2)
        };

        for (int i = 0; i < args.Length; i+=2)
        {
            channel.Samples.Add(new Sample() { ReferenceValue = args[i], Paremeter = args[i + 1] });
        }

        var coefficients = LeastSquareMethod.GetCoeffitients(channel);
        
        
        for (int i = 0; i <  coefficients.Length; i++)
            Assert.True(IsInBounds(expected[i], coefficients[i], 0.005));
    }

    private bool IsInBounds(double expected, double arg, double error)
    {
        if (arg * expected < 0)
            return false;
        return Math.Abs(arg) >= Math.Abs(expected - expected * error) && Math.Abs(arg) <= Math.Abs(expected + expected * error);
    }
}
