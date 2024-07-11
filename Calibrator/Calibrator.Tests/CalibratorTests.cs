namespace Calibrator.Tests
{
    public class CalibratorTests
    {
        [Fact]
        public void CalibratesCorrectly(double[] references, double[] parameters)
        {
            var testHelper = new TestHelper();
            var report = testHelper.TestReport;

            var calibrator = new Domain.Model.Calibrator.Calibrator();

        }
    }
}