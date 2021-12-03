using NUnit.Framework;
using FluentAssertions;
using System.Linq;

namespace day1.tests;

public class Day1Part2Tests
{
    private TripletSumCounter counter = new TripletSumCounter();

    [Test]
    public void SingleTripletHasNoneLarger()
    {
        counter.CountConsecutiveIncreases(new[] {100}).Should().Be(0);
        counter.CountConsecutiveIncreases(new[] {100, 101}).Should().Be(0);
        counter.CountConsecutiveIncreases(new[] {100, 101, 102}).Should().Be(0);
    }

    [Test]
    public void TwoTripletsWithOneLarger()
    {
        counter.CountConsecutiveIncreases(
            new[]{
                100, 101, 102,
                100, 101, 103
            }).Should().Be(1);
    }

    [Test]
    public void ThreeTripletsWithTwoLargerInTheMiddle()
    {
        counter.CountConsecutiveIncreases(
            new[]{
                100, 101, 102,
                100, 101, 103,
                100, 101, 104,
                100, 101, 102,
            }).Should().Be(2);
    }

    [Test]
    public void GoldenTest()
    {
        var lines = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var measurements = lines.Select(l => int.Parse(l)).ToArray();
        counter.CountConsecutiveIncreases(measurements).Should().Be(1344);
    }
}