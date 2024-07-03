namespace Calibrator.Domain.Model.Approximator;

public static class Matrix
{
    public static double GetDeterminant(double[][] matrix)
    {
        if (matrix.Length == 1)
            return matrix[0][0];
        else if (matrix.Length == 2)
            return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];

        double sum = 0;
        for (int i = 0; i < matrix.Length; i++)
        {
            int sign = i % 2 == 0 ? 1 : -1;

            double[][] minor = new double[matrix.Length - 1][];
            for (int k = 1; k < matrix.Length; k++)
            {
                int count = 0;
                double[] temp = new double[minor.Length];
                for (int l = 0; l < matrix.Length; l++)
                {
                    if (k != 0 && l != i)
                    {
                        temp[count] = matrix[k][l];
                        count++;
                    }
                }
                minor[k - 1] = temp;
            }
            sum += sign * matrix[0][i] * GetDeterminant(minor);
        }
        return sum;
    }
    public static double[] GetCoeffitients(Report.SensorChannel channel)
    {
        double[] X = new double[channel.Parameters.Count];
        double[] Y = new double[channel.Parameters.Count];
        int i = 0;
        foreach (var parameter in channel.Parameters)
        {
            X[i] = parameter.Key.ReferenceValue;
            Y[i] = parameter.Value;
            i++;
        }

        double[] interceptTerms = new double[3]; //матрица свободных членов
        double[][] m = new double[3][];

        for (int j = 0; j < 3; j++) //заполнение матрицы свободных членов
        {
            for (int k = 0; k < X.Length; k++)
                interceptTerms[j] += Math.Pow(X[k], 2 - j) * Y[k];
        }

        for (int j = 0; j < 3; j++) //заполнение матрицы m
        {
            double[] t = new double[3];
            for (int l = 0; l < 3; l++)
            {
                for (int k = 0; k < X.Length; k++)
                {
                    t[l] += Math.Pow(X[k], 4 - l - j);
                }

                if (j == 2 && l == 2)
                    t[l] = X.Length;
            }
            m[j] = t;
        }

        double[][] ma = m;
        double[][] mb = m;
        double[][] mc = m;

        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                switch (j)
                {
                    case 0:
                        ma[k][0] = interceptTerms[k];
                        break;
                    case 1:
                        mb[k][1] = interceptTerms[k];
                        break;
                    case 2:
                        mc[k][2] = interceptTerms[k];
                        break;
                }
            }
        }
        double deltaM = GetDeterminant(m);
        double[] result = [GetDeterminant(ma) / deltaM, GetDeterminant(mb) / deltaM, GetDeterminant(mc) / deltaM];
        return result;
    }
}
