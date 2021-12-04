using NUnit.Framework;
using FluentAssertions;

namespace day2.tests;

public class Day2Part2Tests
{
    [Test]
    public void PositionStartsAt0()
    {
        var pos = GetPosition(new string[] {});
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(0);
        pos.Aim.Should().Be(0);
    }

    [TestCase(1)]
    [TestCase(5)]
    public void ForwardCommandChangesHorizontalPositionByUnitsSpecified(int units)
    {
        var commands = new [] { "forward " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(units);
        pos.Aim.Should().Be(0);
    }

    [TestCase(0, 1)]
    [TestCase(0, 5)]
    [TestCase(1, 1)]
    [TestCase(1, 5)]
    public void ForwardCommandIncreasesDepthByAimMultipliedByUnits(int aim, int units)
    {
        PositionAndAim initialPosition = new PositionAndAim
        {
            Horizontal = 0,
            Depth = 0,
            Aim = aim
        };
        
        var commands = new [] { "forward " + units };
        var pos = GetPosition(commands, initialPosition);
        pos.Depth.Should().Be(aim * units);
        pos.Aim.Should().Be(aim);
    }
    
    [TestCase(1)]
    [TestCase(5)]
    public void DownCommandIncreasesAimByUnitsSpecified(int units)
    {
        var commands = new [] { "down " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(0);
        pos.Aim.Should().Be(units);
    }

    [TestCase(1)]
    [TestCase(5)]
    public void UpCommandDecreasesDepthPositionByUnitsSpecified(int units)
    {
        var commands = new [] { "up " + units };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(0);
        pos.Depth.Should().Be(0);
        pos.Aim.Should().Be(-units);
    }
    
    [Test]
    public void SeriesOfCommandsUpdatesPositionCorrectly()
    {
        var commands = new []
        {
            "down 2",
            "forward 5",
            "up 1"
        };
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(5);
        pos.Depth.Should().Be(10);
        pos.Aim.Should().Be(1);
    }

    [Test]
    public void GoldenTest()
    {
        var commands = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var pos = GetPosition(commands);
        pos.Horizontal.Should().Be(1971);
        pos.Depth.Should().Be(904018);
    }

    private PositionAndAim GetPosition(string[] commands)
    {
        return new PositionAndAimUpdater().GetPosition(commands);
    }

    private PositionAndAim GetPosition(string[] commands, PositionAndAim initialPosition)
    {
        return new PositionAndAimUpdater().GetPosition(commands, initialPosition);
    }
}