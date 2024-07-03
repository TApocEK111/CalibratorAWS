namespace Calibrator.Tests;

public class MatrixTests
{
    [Theory]
    [InlineData(204, new double[] { 1, -2, 3, 4, 0, 6, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 4, 0, 6, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 0, 0, 0, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 4, 0, 6, 0, 0, 0 })]
    [InlineData(0, new double[] { 2, 0, 3, 4, 0, 6, 1, -20, 2})]
    [InlineData(new double[][] { new double[] { 1, -2, 3 }, new double[] { 4, 0, 6 } })]

    public void CalculatesCorrectDeterminant(double[][] expected, double[][] values )
    {
        double[][] matrix = new double[3][];
        int p = 
        for (int i = 0; i < values.Length; i++)
        {
            var row = new double[3];
            for (int j = 0; j < 3; j++)
            {
                row[j] = 
            }
        }
    }


}
