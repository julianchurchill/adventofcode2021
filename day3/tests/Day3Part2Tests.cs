using NUnit.Framework;
using FluentAssertions;

namespace day3.tests;

public class Day3Part2Tests
{
    [Test]
    public void NoEntriesGivesZeroRates()
    {
        var p = ParseBinaryDiagnostics(new string[] {});
        p.O2GeneratorRating.Should().Be(0);
        p.CO2ScrubberRating.Should().Be(0);
    }

    [Test]
    public void O2GeneratorRatingForSingleDigitDiagnostic()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1" });
        p.O2GeneratorRating.Should().Be(0b1);
    }

    [Test]
    public void O2GeneratorRatingForSingleDigitDiagnostics1Beats0InATie()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1", "0" });
        p.O2GeneratorRating.Should().Be(0b1);
    }

    [Test]
    public void O2GeneratorRatingForSingleDigitDiagnosticsMoreOfSameDigitWins()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1", "0", "0" });
        p.O2GeneratorRating.Should().Be(0b0);
    }

    [Test]
    public void O2GeneratorRatingForMultiDigitDiagnostics1Beats0InATie()
    {
        var p = ParseBinaryDiagnostics(new string[] 
        {
            "100",
            "111",
            "110"
        });
        p.O2GeneratorRating.Should().Be(0b111);
    }

    [Test]
    public void CO2ScrubberRatingForSingleDigitDiagnostic()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1" });
        p.CO2ScrubberRating.Should().Be(0b1);
    }

    [Test]
    public void CO2ScrubberRatingForSingleDigitDiagnostics0Beats1InATie()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1", "0" });
        p.CO2ScrubberRating.Should().Be(0b0);
    }
    
    [Test]
    public void CO2ScrubberRatingForSingleDigitDiagnosticsLessOfSameDigitWins()
    {
        var p = ParseBinaryDiagnostics(new string[] { "1", "0", "0" });
        p.CO2ScrubberRating.Should().Be(0b1);
    }
    
    [Test]
    public void CO2ScrubberRatingForMultiDigitDiagnostics0Beats1InATie()
    {
        var p = ParseBinaryDiagnostics(new string[] 
        {
            "100",
            "100",
            "101",
            "101",
            "110",
            "000",
            "001",
            "010",
            "011"
        });
        p.CO2ScrubberRating.Should().Be(0b000);
    }

    [Test]
    public void GoldenTest()
    {
        var diagnostics = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var results = ParseBinaryDiagnostics(diagnostics);
        results.O2GeneratorRating.Should().Be(0b000111111101);
        results.O2GeneratorRating.Should().Be(509);
        results.CO2ScrubberRating.Should().Be(0b101010000101);
        results.CO2ScrubberRating.Should().Be(2693);
    }

    private Diagnostics ParseBinaryDiagnostics(string[] diagnostics)
    {
        return new BinaryDiagnosticsParser().Parse(diagnostics);
    }
}