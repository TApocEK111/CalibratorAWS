namespace Calibrator.Domain.Model.Report;

public class SensorChannel
{
    private int revIndex = 0;
    private List<Sample> _forward;
    private List<Sample> _reverse;
    private List<AverageSample> _avgSamples;

    public List<Sample> Samples { get; set; } = new List<Sample>();
    public List<AverageSample> AvgSamples
    {
        get
        {
            if (_avgSamples != null)
                return _avgSamples;
            if (Samples == null)
                throw new NullReferenceException("Samples are null.");
            else
            {
                _avgSamples = new List<AverageSample>();
                Dictionary<double, double[]> uniques = new Dictionary<double, double[]>();

                for (int i = 0; i < Samples.Count; i++)
                {
                    if (!uniques.TryAdd(Samples[i].ReferenceValue, [Samples[i].Parameter, 1]))
                    {
                        uniques[Samples[i].ReferenceValue][0] += Samples[i].Parameter;
                        uniques[Samples[i].ReferenceValue][1] ++;
                    }
                }

                foreach (var sample in uniques)
                {
                    AverageSample tempSample = new AverageSample();
                    tempSample.ReferenceValue = sample.Key;
                    tempSample.Parameter = sample.Value[0] / sample.Value[1];
                    _avgSamples.Add(tempSample);
                }

                return _avgSamples;
            }
        }
        set
        {
            _avgSamples = value;
        }
    }
    public PhisicalQuantity PhisicalQuantity { get; set; }
    public List<Sample> Forward 
    { 
        get 
        {
            if (_forward != null)
                return _forward;
            if (Samples == null)
                throw new NullReferenceException("Samples are null.");
            else if (revIndex == 0)
            {
                revIndex = 1;
                while (Samples[revIndex].ReferenceValue == Samples[revIndex - 1].ReferenceValue)
                    revIndex++;
                double sign = Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue;
                if (sign < 0)
                {
                    while (revIndex < Samples.Count && Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue <= 0)
                        revIndex++;
                }
                else
                {
                    while (revIndex < Samples.Count && Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue >= 0)
                        revIndex++;
                }
            }
            _forward = new List<Sample>(revIndex);
            for (int i = 0; i < revIndex; i++)
            {
                _forward.Add(Samples[i]);
            }
            return _forward;
        }
        private set
        {
            _forward = value;
        }
    }
    public List<Sample> Reverse 
    {
        get
        {
            if (_reverse != null)
                return _reverse;
            if (Samples == null)
                throw new NullReferenceException("Samples are null.");
            else if (revIndex == 0)
            {
                revIndex = 1;
                while (Samples[revIndex].ReferenceValue == Samples[revIndex - 1].ReferenceValue)
                    revIndex++;
                double sign = Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue;
                if (sign < 0)
                {
                    while (revIndex < Samples.Count && Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue <= 0)
                        revIndex++;
                }
                else
                {
                    while (revIndex < Samples.Count && Samples[revIndex].ReferenceValue - Samples[revIndex - 1].ReferenceValue >= 0)
                        revIndex++;
                }
            }
            _reverse = new List<Sample>(Samples.Count - 1 - revIndex);
            for (int i = revIndex; i < Samples.Count; i++)
            {
                _reverse.Add(Samples[i]);
            }
            return _reverse;
        }
        private set
        {
            _reverse = value;
        }
    }
}
