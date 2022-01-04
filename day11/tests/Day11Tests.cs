using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace day11;

public class Day11Tests
{
    [Test]
    public void SingleOctopusGains1EnergyEachStep()
    {
        var inputStep1 = new List<string> { "0" };
        var expectedOutputStep1 = new List<string> { "1" };
        OctopiStepOnce(inputStep1.ToIntGrid()).EnergyLevels.Should().BeEquivalentTo(expectedOutputStep1.ToIntGrid());

        var inputStep2 = new List<string> { "1" };
        var expectedOutputStep2 = new List<string> { "2" };
        OctopiStepOnce(inputStep2.ToIntGrid()).EnergyLevels.Should().BeEquivalentTo(expectedOutputStep2.ToIntGrid());
    }

    [Test]
    public void FlashingOctopusIsReset()
    {
        var input = new List<string> { "9" };
        var expectedOutput = new List<string> { "0" };
        OctopiStepOnce(input.ToIntGrid()).EnergyLevels.Should().BeEquivalentTo(expectedOutput.ToIntGrid());
    }

    [Test]
    public void FlashingOctopusAddsEnergyToAdjacentOctopi()
    {
        var input = new List<string> { "00000", "00900", "00000" };
        var expectedOutput = new List<string> { "12221", "12021", "12221" };
        OctopiStepOnce(input.ToIntGrid()).EnergyLevels.Should().BeEquivalentTo(expectedOutput.ToIntGrid());
    }

    [Test]
    public void FlashingOctopusCanCauseChainReaction()
    {
        var input = new List<string> { "980" };
        var expectedOutput = new List<string> { "002" };
        OctopiStepOnce(input.ToIntGrid()).EnergyLevels.Should().BeEquivalentTo(expectedOutput.ToIntGrid());
    }

    [Test]
    public void GoldenInputTestPart1()
    {
        var lines = LoadGoldenInput().ToList();
        var input = lines.ToIntGrid();
        var flashCount = 0;
        for(int i = 0; i < 100; i++)
        {
            var result = OctopiStepOnce(input);
            input = result.EnergyLevels;
            flashCount += result.FlashCount;
        }
        flashCount.Should().Be(1694);
    }

    [Test]
    public void GoldenInputTestPart2()
    {
        var lines = LoadGoldenInput().ToList();
        var input = lines.ToIntGrid();
        var numberOfOctopi = input.Count * input[0].Count;
        var stepCount = 0;
        while(true)
        {
            stepCount++;
            var result = OctopiStepOnce(input);
            if(result.FlashCount == numberOfOctopi)
            {
                break;
            }
            input = result.EnergyLevels;
        }
        stepCount.Should().Be(346);
    }

    private string[] LoadGoldenInput()
    {
        return System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
    }

    OctopiStepResult OctopiStepOnce(List<List<int>> input)
    {
        var octopi = ToOctopi(input);
        IncreaseOctopusEnergy(octopi);
        var flashCount = UpdateNeighboursAndCountFlashes(octopi);
        ResetFlashedOctopi(octopi);
        return new OctopiStepResult(IntGridFromOctopi(input.Count, input[0].Count, octopi), flashCount);
    }

    class OctopiStepResult
    {
        public OctopiStepResult(List<List<int>> energylevels, int flashCount)
        {
            EnergyLevels = energylevels;
            FlashCount = flashCount;
        }

        public readonly List<List<int>> EnergyLevels;
        public readonly int FlashCount;
    }

    private static int UpdateNeighboursAndCountFlashes(Dictionary<(int, int), Octopus> octopi)
    {
        var alreadyFlashed = new HashSet<(int, int)>();
        while(true)
        {
            var flashingOctopi = octopi.Values
                .Where(o => o.EnergyLevel > 9 && !alreadyFlashed.Contains(o.Location))
                .ToList();
            if(flashingOctopi.Count == 0)
            {
                return alreadyFlashed.Count;
            }
            foreach (var octopus in flashingOctopi)
            {
                alreadyFlashed.Add(octopus.Location);
                var loc = octopus.Location;
                UpdatePotentialNeighbour(octopi, (loc.Item1 - 1, loc.Item2));
                UpdatePotentialNeighbour(octopi, (loc.Item1 + 1, loc.Item2));
                UpdatePotentialNeighbour(octopi, (loc.Item1, loc.Item2 - 1));
                UpdatePotentialNeighbour(octopi, (loc.Item1, loc.Item2 + 1));
                UpdatePotentialNeighbour(octopi, (loc.Item1 - 1, loc.Item2 - 1));
                UpdatePotentialNeighbour(octopi, (loc.Item1 + 1, loc.Item2 - 1));
                UpdatePotentialNeighbour(octopi, (loc.Item1 - 1, loc.Item2 + 1));
                UpdatePotentialNeighbour(octopi, (loc.Item1 + 1, loc.Item2 + 1));
            }
        }
    }

    private static void UpdatePotentialNeighbour(Dictionary<(int, int), Octopus> octopi, (int, int) neighbourLocation)
    {
        if (octopi.TryGetValue(neighbourLocation, out var neighbour))
        {
            neighbour.EnergyLevel++;
        }
    }

    private static void IncreaseOctopusEnergy(Dictionary<(int, int), Octopus> octopi)
    {
        foreach (var octopus in octopi.Values)
        {
            octopus.EnergyLevel++;
        }
    }

    private static void ResetFlashedOctopi(Dictionary<(int, int), Octopus> octopi)
    {
        foreach (var octopus in octopi.Values)
        {
            if (octopus.EnergyLevel > 9)
            {
                octopus.EnergyLevel = 0;
            }
        }
    }

    private List<List<int>> IntGridFromOctopi(int rowCount, int colCount, Dictionary<(int, int), Octopus> octopi)
    {
        var output = new List<List<int>>();
        for(int row = 0; row < rowCount; row++)
        {
            var newRow = new List<int>();
            output.Add(newRow);
            for(int col = 0; col < colCount; col++)
            {
                newRow.Add(0);
            }
        }
        foreach(var octopus in octopi.Values)
        {
            output[octopus.Location.Item1][octopus.Location.Item2] = octopus.EnergyLevel;
        }
        return output;
    }

    private Dictionary<(int, int), Octopus> ToOctopi(List<List<int>> input)
    {
        var octopi = new Dictionary<(int, int), Octopus>();
        var rowIndex = 0;
        foreach(var row in input)
        {

            var colIndex = 0;
            foreach(var point in row)
            {
                var location = (rowIndex, colIndex);
                octopi.Add(location, new Octopus { EnergyLevel = point, Location = location });
                colIndex++;
            }
            rowIndex++;
        }
        return octopi;
    }

    class Octopus
    {
        public (int,int) Location;
        public int EnergyLevel;
    }
}

static class ListStringExtensions
{
    public static List<List<int>> ToIntGrid(this List<string> input)
    {
        var output = new List<List<int>>();
        foreach(var line in input)
        {
            var outputLine = new List<int>();
            foreach(var c in line)
            {
                outputLine.Add(int.Parse(c.ToString()));
            }
            output.Add(outputLine);
        }
        return output;
    }
}

