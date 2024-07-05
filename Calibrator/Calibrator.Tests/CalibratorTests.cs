namespace Calibrator.Tests
{
    public class CalibratorTests
    {
        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 4, 3, 2, 1 }, new double[] { 101, 199, 302, 400, 501, 398, 301, 200, 98 })]
        public void CalibratesCorrectly(double[] references, double[] parameters)
        {
            var report = new Report() { Sensors = new List<Sensor> { new Sensor() { Channels = new List<SensorChannel>() { new SensorChannel() {Samples = new List<Sample>(parameters.Length) } } } } };
            for (int i = 0; i < parameters.Length; i++)
            {
                report.Sensors[0].Channels[0].Samples.Add(new Sample() { ReferenceValue = references[i], Parameter = parameters[i] });
            }

            Domain.Model.Calibrator.Calibrator.CalculateTargetPhisicalValues(report);

            int j = 0;
            foreach (var sample in report.Sensors[0].Channels[0].Samples)
            {
                Assert.True(IsInBounds(references[j], (double)sample.ResultantPhysicalQuantity, 0.2));
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
}