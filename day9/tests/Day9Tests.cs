using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace day9;

public class Day9Tests
{
    [Test]
    public void EmptyHeightMapMeansNoLowPoints()
    {
        SumLowPointValues(new int[][] {}).Should().Be(0);
    }

    [Test]
    public void SingleValueHeightMapMeansOneLowPoint()
    {
        SumLowPointValues(new int[][] { new int[] {1} }).Should().Be(1);
        SumLowPointValues(new int[][] { new int[] {2} }).Should().Be(2);
    }

    // test cases
    // x 2,1
    // x 1,1
    // x 1,2,1
    // x    1,2,1
    // x    2,1,2
    // 1,3,2
    // 1,3,2,2
    // 1,3,2,1    // optional
    // 1,3,2,2,1

    [Test]
    public void TwoValueMapLowestPointWins()
    {
        SumLowPointValues(new int[][] { new int[] {3,2} }).Should().Be(2);
    }

    [Test]
    public void TwoValueMapWithEqualLowPointsAreSummed()
    {
        SumLowPointValues(new int[][] { new int[] {2,2} }).Should().Be(4);
    }

    [Test]
    public void ThreeValueMapWithPeakSeparatingEqualMinima()
    {
        SumLowPointValues(new int[][] { new int[] {1,2,1} }).Should().Be(2);
    }

    [Test]
    public void TwoDimensionalMapWithPeaksSeparatingEqualMinima()
    {
        SumLowPointValues(new int[][] {
            new int[] {1,2,1},
            new int[] {2,1,2}
        }).Should().Be(3);
    }

    [Test]
    public void ThreeValueMapWithPeakAndTwoMinima()
    {
        // Test to force summing of multiple levels of minima
        SumLowPointValues(new int[][] { new int[] {1,3,2} }).Should().Be(3);
    }

    private int SumLowPointValues(int[][] input)
    {
        if(input.Length == 0)
        {
            return 0;
        }
        var minValue = FindGlobalMinimumValue(input);
        int total = 0;
        foreach(var row in input)
        {
            total += row.Where(v => v == minValue).Sum();
        }
        return total;
    }

    private int FindGlobalMinimumValue(int[][] input)
    {
        var globalMinimum = int.MaxValue;
        foreach(var row in input)
        {
            var rowMin = row.Min();
            if(rowMin < globalMinimum) globalMinimum = rowMin;
        }
        return globalMinimum;
    }
}