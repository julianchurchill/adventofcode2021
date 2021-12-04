using NUnit.Framework;
using FluentAssertions;

namespace day2.tests;

public class Day2Part1Tests
{
    [Test]
    public void PositionStartsAt0()
    {
        var pos = GetPosition(new string[] {});
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(0);
    }

    [TestCase(1)]
    [TestCase(5)]
    public void ForwardCommandChangesHorizontalPositionByUnitsSpecified(int units)
    {
        var commands = new [] { "forward " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(units);
        pos.Depth.Should().Be(0);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public void DownCommandIncreasesDepthPositionByUnitsSpecified(int units)
    {
        var commands = new [] { "down " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(units);
    }

    [TestCase(1)]
    [TestCase(5)]
    public void UpCommandDecreasesDepthPositionByUnitsSpecified(int units)
    {
        var commands = new [] { "up " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(-units);
    }
    
    [Test]
    public void SeriesOfCommandsUpdatesPositionCorrectly()
    {
        var commands = new []
        {
            "forward 5",
            "down 2",
            "up 1"
        };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(5);
        pos.Depth.Should().Be(1);
    }

    [Test]
    public void GoldenTest()
    {
        var commands = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(1971);
        pos.Depth.Should().Be(830);
    }

    private Position GetPosition(string[] commands)
    {
        return new PositionUpdater().GetPosition(commands);
    }
}