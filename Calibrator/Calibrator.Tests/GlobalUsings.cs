global using Xunit;
global using Calibrator.Domain.Model.Report;
global using Calibrator.Domain.Model.Calibrator;

internal bool IsInBounds(double expected, double arg, double error)
{
    if (arg * expected < 0)
        return false;
    return Math.Abs(arg) >= Math.Abs(expected - expected * error) && Math.Abs(arg) <= Math.Abs(expected + expected * error);
}