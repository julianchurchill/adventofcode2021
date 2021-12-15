using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace day6;

public class Day6Tests
{
    [Test]
    public void NoFishYieldsNoFish()
    {
        SlowSimulateOneDay(new int[] {}).Should().BeEquivalentTo(new int[] {});
        FastSimulateOneDay(
            new Dictionary<int, long> {{0,0},{1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}})
            .Should().BeEquivalentTo(
            new Dictionary<int, long> {{0,0},{1,0},{2,0},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}});
    }

    [Test]
    public void FishCountdown()
    {
        SlowSimulateOneDay(new int[] {3, 4}).Should().BeEquivalentTo(new int[] {2, 3});
        FastSimulateOneDay(
            new Dictionary<int, long> {{0,0},{1,0},{2,0},{3,1},{4,1},{5,0},{6,0},{7,0},{8,0}})
            .Should().BeEquivalentTo(
            new Dictionary<int, long> {{0,0},{1,0},{2,1},{3,1},{4,0},{5,0},{6,0},{7,0},{8,0}});
    }

    [Test]
    public void FishCountdownAt0GoesTo6AndMakesNewFish()
    {
        SlowSimulateOneDay(new int[] {0, 2}).Should().BeEquivalentTo(new int[] {6, 1, 8});
        FastSimulateOneDay(
            new Dictionary<int, long> {{0,1},{1,0},{2,1},{3,0},{4,0},{5,0},{6,0},{7,0},{8,0}})
            .Should().BeEquivalentTo(
            new Dictionary<int, long> {{0,0},{1,1},{2,0},{3,0},{4,0},{5,0},{6,1},{7,0},{8,1}});
    }

    [Test]
    public void GoldenTest80Days()
    {
        var originalFish = LoadGoldenInput();
        var nextDayOfFish = SlowSimulate(originalFish, 80);
        nextDayOfFish.Should().Be(365862);
    }

    [Test]
    public void GoldenTest256Days()
    {
        var originalFish = LoadGoldenInput();
        var nextDayOfFish = FastSimulate(originalFish, 256);
        nextDayOfFish.Should().Be(1653250886439);
    }

    private int[] LoadGoldenInput()
    {
        var input = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        return input[0].Split(",").Select(l => int.Parse(l)).ToArray();
    }

    private long SlowSimulate(int[] lanternFish, int days = 1)
    {
        var nextDayOfFish = lanternFish;
        for(int i = 0; i < days; i++)
        {
            nextDayOfFish = SlowSimulateOneDay(nextDayOfFish);
        }
        return nextDayOfFish.Count();
    }

    private long FastSimulate(int[] lanternFish, int days = 1)
    {
        var nextDayOfFish = new Dictionary<int, long>();
        nextDayOfFish.Add(0, lanternFish.Count(l => l == 0));
        nextDayOfFish.Add(1, lanternFish.Count(l => l == 1));
        nextDayOfFish.Add(2, lanternFish.Count(l => l == 2));
        nextDayOfFish.Add(3, lanternFish.Count(l => l == 3));
        nextDayOfFish.Add(4, lanternFish.Count(l => l == 4));
        nextDayOfFish.Add(5, lanternFish.Count(l => l == 5));
        nextDayOfFish.Add(6, lanternFish.Count(l => l == 6));
        nextDayOfFish.Add(7, lanternFish.Count(l => l == 7));
        nextDayOfFish.Add(8, lanternFish.Count(l => l == 8));
        for(int i = 0; i < days; i++)
        {
            nextDayOfFish = FastSimulateOneDay(nextDayOfFish);
        }
        return nextDayOfFish.Sum(kvp => kvp.Value);
    }

    private Dictionary<int, long> FastSimulateOneDay(Dictionary<int, long> fishCount)
    {
        var newFish = fishCount[0];
        fishCount[0] = fishCount[1];
        fishCount[1] = fishCount[2];
        fishCount[2] = fishCount[3];
        fishCount[3] = fishCount[4];
        fishCount[4] = fishCount[5];
        fishCount[5] = fishCount[6];
        fishCount[6] = fishCount[7];
        fishCount[6] += newFish;
        fishCount[7] = fishCount[8];
        fishCount[8] = newFish;
        return fishCount;
    }

    private int[] SlowSimulateOneDay(int[] lanternFish)
    {        
        const int existingFishResetValue = 6;
        const int newFishValue = 8;
        var newFish = 0;
        var nextDayOfFish = new List<int>();
        foreach(var fish in lanternFish)
        {
            if(fish == 0)
            {
                nextDayOfFish.Add(existingFishResetValue);
                newFish++;
            }
            else
            {
                nextDayOfFish.Add(fish-1);
            }
        }
        nextDayOfFish.AddRange(Enumerable.Repeat(newFishValue, newFish));
        return nextDayOfFish.ToArray();
    }
}