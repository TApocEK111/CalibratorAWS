namespace Calibrator.Tests
{
    public class CalibratorTests
    {
        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 4, 3, 2, 1 }, new double[] { 101, 199, 302, 400, 501, 398, 301, 200, 98 })]
        public void CalibratesCorrectly(double[] references, double[] parameters)
        {

        }

        private bool IsInBounds(double expected, double arg, double error)
        {
            if (arg * expected < 0)
                return false;
            return Math.Abs(arg) >= Math.Abs(expected - expected * error) && Math.Abs(arg) <= Math.Abs(expected + expected * error);
        }
    }
}