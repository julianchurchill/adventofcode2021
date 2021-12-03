using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace day1.tests;

public class Day1Part1Tests
{
    private SingleNumberCounter counter = new SingleNumberCounter();

    [Test]
    public void SingleMeasurementHasNoneLarger()
    {
        counter.CountConsecutiveIncreases(new[] {100}).Should().Be(0);
    }

    [Test]
    public void TwoMeasurementsWithOneLarger()
    {
        counter.CountConsecutiveIncreases(new[] {100, 101}).Should().Be(1);
    }

    [Test]
    public void ThreeMeasurementsWithOneLargerInTheMiddle()
    {
        counter.CountConsecutiveIncreases(new[] {100, 101, 99}).Should().Be(1);
    }

    [Test]
    public void GoldenTest()
    {
        var lines = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var measurements = lines.Select(l => int.Parse(l)).ToArray();
        counter.CountConsecutiveIncreases(measurements).Should().Be(1316);
    }
}