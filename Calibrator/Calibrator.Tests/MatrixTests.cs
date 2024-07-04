namespace Calibrator.Tests;

public class MatrixTests
{
    static SensorChannel sensorChannel = new SensorChannel() { Sensor = new Sensor() { Id = 1 },  Parameters = new Dictionary<Entry, double>()
    {
        { new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = 200 }, 198 },
        { new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = 300 }, 303 },
        { new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = 400 }, 404 },
        { new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = 500 }, 498 },
        { new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = 600 }, 602 }
    }
    };

    [Theory]
    [InlineData(204, new double[] { 1, -2, 3, 4, 0, 6, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 4, 0, 6, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 0, 0, 0, -7, 8, 9 })]
    [InlineData(0, new double[] { 0, 0, 0, 4, 0, 6, 0, 0, 0 })]
    [InlineData(0, new double[] { 2, 0, 3, 4, 0, 6, 1, -20, 2})]

    public void CalculatesCorrectDeterminant(double expected, double[] values )
    {
        double[][] matrix = new double[3][];
        int p = 0;
        for (int i = 0; i < 3; i++)
        {
            var row = new double[3];
            for (int j = 0; j < 3; j++)
            {
                row[j] = values[p];
                p++;
            }
            matrix[i] = row;
        }

        var determinant = Matrix.GetDeterminant(matrix);

        Assert.Equal(expected, determinant);
    }

    [Theory]
    [InlineData(new double[] { -0.000064, 1.05, 9.2 }, new double[] { 200, 198, 300, 303, 400, 404, 500, 498, 600, 602 })]
    public void CalculatesCorrectCoefficients(double[] expected, double[] args)
    {
        SensorChannel channel = new SensorChannel()
        {
            Sensor = new Sensor() { Id = 1 },
            Parameters = new Dictionary<Entry, double>()
        };

        for (int i = 0; i < args.Length; i+=2)
        {
            channel.Parameters.Add(new Entry() { Start = DateTime.Now, End = DateTime.Now, ReferenceValue = args[i] }, args[i + 1]);
        }

        var coefficients = Matrix.GetCoeffitients(channel);
        
        
        for (int i = 0; i <  coefficients.Length; i++)
            Assert.True(IsInBounds(expected[i], coefficients[i]));
    }

    private bool IsInBounds(double expected, double arg)
    {
        return arg > expected * 0.8 && arg < expected * 1.2;
    }
}
