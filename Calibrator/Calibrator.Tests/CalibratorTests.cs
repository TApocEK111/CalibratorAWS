namespace Calibrator.Tests
{
    public class CalibratorTests
    {
        [Theory]

        public void CalibratesCorrectly(double[] references, double[] parameters)
        {
            var report = new Report() { Sensors = new List<Sensor> { new Sensor() { Channels = new List<SensorChannel>() { new SensorChannel() {Samples = new List<Sample>(parameters.Length) } } } } };
            for (int i = 0; i < parameters.Length; i++)
            {
                report.Sensors[0].Channels[0].Samples.Add(new Sample() { ReferenceValue = references[i], Paremeter = parameters[i] });
            }

            Domain.Model.Calibrator.Calibrator.CalculateTargetPhisicalValues(report);

            int j = 0;
            foreach (var sample in report.Sensors[0].Channels[0].Samples)
            {
                Assert.True(IsInBounds(references[j], sample.ReferenceValue, 0.2));
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