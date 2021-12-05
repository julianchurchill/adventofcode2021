using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace day3.tests;

public class Day3Part1Tests
{
    [Test]
    public void NoEntriesGivesZeroRates()
    {
        var p = ParseBinaryDiagnostics(new string[] {});
        p.GammaRate.Should().Be(0);
        p.EpsilonRate.Should().Be(0);
    }

    [Test]
    public void EpsilonRateIsInverseOfGamma()
    {
        var p = ParseBinaryDiagnostics(new string[] { "010101" });
        p.EpsilonRate.Should().Be(0b101010);
    }

    [Test]
    public void SingleEntryIsGammaRate()
    {
        var p = ParseBinaryDiagnostics(new string[] { "010101" });
        p.GammaRate.Should().Be(0b010101);
    }

    [Test]
    public void ThreeEntriesFindsGammaRateToUseMostCommonBits()
    {
        var p = ParseBinaryDiagnostics(new string[]
        {
            "010101",
            "011111",
            "001101"
        });
        p.GammaRate.Should().Be(0b011101);
    }

    [Test]
    public void GoldenTest()
    {
        var diagnostics = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var results = ParseBinaryDiagnostics(diagnostics);
        results.GammaRate.Should().Be(0b000011000111);
        results.GammaRate.Should().Be(199);
        results.EpsilonRate.Should().Be(0b111100111000);
        results.EpsilonRate.Should().Be(3896);
    }

    private Diagnostics ParseBinaryDiagnostics(string[] diagnostics)
    {
        return new BinaryDiagnosticsParser().Parse(diagnostics);
    }
}