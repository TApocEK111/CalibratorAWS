namespace Calibrator.Tests
{
    public class ApproximatorTests
    {
        [Theory]

        public void ApproximatesValuesCorrectly(Dictionary<Experiment, double> expected, SensorChannel channel)
        {
            Approximator.AproximateResults(channel);

            Assert.Equal(expected, channel.Results);
        }
    }
}