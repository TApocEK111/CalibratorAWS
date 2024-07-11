namespace Calibrator.Tests;

public class TestHelper
{
    public Report TestReport = new Report
    {
        Sensors = new List<Sensor>
            {
                new Sensor
                {
                    Channels = new List<SensorChannel>()
                    {
                        new SensorChannel()
                        {
                            Samples = new List<Sample>()
                            {
                                new Sample() { ReferenceValue = 50, Parameter = 51 },
                                new Sample() { ReferenceValue = 50, Parameter = 48 },
                                new Sample() { ReferenceValue = 60, Parameter = 62 },
                                new Sample() { ReferenceValue = 60, Parameter = 59 },
                                new Sample() { ReferenceValue = 70, Parameter = 70 },
                                new Sample() { ReferenceValue = 50, Parameter = 51 }
                            }
                        }
                    }
                }
            }
    };

}
