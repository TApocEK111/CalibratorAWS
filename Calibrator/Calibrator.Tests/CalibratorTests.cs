namespace Calibrator.Tests
{
    public class CalibratorTests
    {
        [Fact]
        public void CalibratesCorrectly()
        {
            var testHelper = new TestHelper();
            var report = testHelper.TestReport;

            var calibrator = new Domain.Model.Calibrator.Calibrator();
            calibrator.CalculatePhysicalQuantitylValues(report);

            foreach (var sample in report.Sensors[0].Channels[0].Samples)
            {
                Assert.Equal(report.Sensors);
            }

        }
    }
}