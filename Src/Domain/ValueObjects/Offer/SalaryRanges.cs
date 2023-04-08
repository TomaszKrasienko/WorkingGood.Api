namespace Domain.ValueObjects.Offer;

public class SalaryRanges
{
    public double ValueMin { get; private set; }
    public double ValueMax { get; private set; }
    public SalaryRanges(double valueMin, double valueMax)
    {
        ValueMin = valueMin;
        ValueMax = valueMax;
    }
}