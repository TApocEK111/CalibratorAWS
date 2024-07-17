﻿namespace Calibrator.Domain.Model.Setpoint;

public class Exposure
{
    public Guid Id { get; set; }
    public int Number { get; set; }

    public double Value { get; set; }
    public double Speed { get; set; }
    public double Duration { get; set; }

    public Setpoint Setpoint { get; set; }
}