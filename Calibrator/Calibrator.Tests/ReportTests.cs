namespace Calibrator.Tests;

public class ReportTests
{
    [Theory]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, new double[] { 4, 3, 2, 1 }, new double[] { 1, 2, 3, 4, 5, 4, 3, 2, 1})]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, new double[] { 4, 4, 4, 3, 2, 1, 1 }, new double[] { 1, 2, 3, 4, 5, 4, 4, 4, 3, 2, 1, 1 })]
    [InlineData(new double[] { 1, 1, 1, 2, 2, 3, 4, 5, 5 }, new double[] { 4, 3, 2, 1 }, new double[] { 1, 1, 1, 2, 2, 3, 4, 5, 5, 4, 3, 2, 1 })]
    [InlineData(new double[] { 1, 2, 3, 4, 5 }, new double[] {  }, new double[] { 1, 2, 3, 4, 5 })]
    public void CorrectForwardReverseLists(double[] fExpected, double[] rExpected, double[] list)
    {
        var channel = new SensorChannel() { Samples = new List<Sample>(list.Length) };

        for (int i = 0; i < list.Length; i++)
        {
            channel.Samples.Add(new Sample() { ReferenceValue = list[i] });
        }

        var forward = channel.Forward;
        var reverse = channel.Reverse;
        for (int i = 0; i < fExpected.Length; i++)
        {
            Assert.Equal(forward[i].ReferenceValue, fExpected[i]);
        }
        for (int i = 0; i < rExpected.Length; i++)
        {
            Assert.Equal(reverse[i].ReferenceValue, rExpected[i]);
        }
    }
}
