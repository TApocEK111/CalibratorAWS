namespace Calibrator.Domain.Model.Report;

public class SensorChannel
{
    private int revIndex = 0;
    private List<Sample> _forward;
    private List<Sample> _reverse;
    private List<Sample> _avgSamples;

    public List<Sample> Samples { get; set; }
    public List<Sample> AvgSamples
    {
        get
        {
            if (_avgSamples != null)
                return _avgSamples;
            if (Samples == null)
                throw new NullReferenceException("Samples are null.");
            else
            {
                _avgSamples = new List<Sample>();
                for (int i = 0, j = Samples.Count - 1; j - i >= 0; i++, j--)
                {
                    Sample tempSample = new Sample();
                    tempSample.ReferenceValue = Samples[i].ReferenceValue;
                    double sum = 0;
                    int counter = 0;
                    while (Samples[i].ReferenceValue == Samples[i + 1].ReferenceValue)
                    {
                        sum += Samples[i].Parameter;
                        sum += Samples[j].Parameter;
                        counter += 2;
                        i++;
                        j--;
                    }
                    tempSample.Parameter = sum / counter;
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
    public SensorChannel()
    {

    }
}
