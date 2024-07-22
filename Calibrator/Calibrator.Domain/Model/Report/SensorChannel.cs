using System.Text.Json.Serialization;

namespace Calibrator.Domain.Model.Report;

public class SensorChannel
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public double MaxError { get; set; }
    public List<Sample> Samples { get; set; } = new List<Sample>();
    public List<AverageSample> AverageSamples { get; set; } = null!;
    public Coefficients Coefficients { get; set; }
    public PhysicalQuantity PhisicalQuantity { get; set; } = PhysicalQuantity.Udefined;

    [JsonIgnore]
    public Sensor Sensor { get; set; }

    public void DefineMaxError() => MaxError = Samples.MaxBy(s => s.Error).Error;

    public void DefineSamplesDirections()
    {
        if (Samples.Count == 0)
            throw new ArgumentException("Samples is empty.");

        Samples[0].Direction = Direction.Forward;
        double forward = Samples[1].ReferenceValue - Samples[0].ReferenceValue;
        for (int i = 1; i < Samples.Count; i++)
        {
            double sign = Samples[i].ReferenceValue - Samples[i - 1].ReferenceValue;
            if (forward > 0)
            {
                if (sign > 0)
                    Samples[i].Direction = Direction.Forward;
                else
                    Samples[i].Direction = Direction.Reverse;
            }
            else
            {
                if (sign < 0)
                    Samples[i].Direction = Direction.Forward;
                else
                    Samples[i].Direction = Direction.Reverse;
            }
        }
    }
    public void CalculateAverageSamples()
    {
        if (Samples == null)
            throw new NullReferenceException("Samples are null.");
        else
        {
            AverageSamples = new List<AverageSample>();
            Dictionary<double, double[]> uniques = new Dictionary<double, double[]>();

            for (int i = 0; i < Samples.Count; i++)
            {
                if (!uniques.TryAdd(Samples[i].ReferenceValue, [Samples[i].Parameter, 1]))
                {
                    uniques[Samples[i].ReferenceValue][0] += Samples[i].Parameter;
                    uniques[Samples[i].ReferenceValue][1]++;
                }
            }

            foreach (var sample in uniques)
            {
                AverageSample tempSample = new AverageSample();
                tempSample.ReferenceValue = sample.Key;
                tempSample.Parameter = sample.Value[0] / sample.Value[1];
                AverageSamples.Add(tempSample);
            }
        }
    }
}
