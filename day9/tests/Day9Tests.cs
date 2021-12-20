using System.Collections.Generic;
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
        SumLowPointValues(new int[][] { new int[] {1} }).Should().Be(2);
        SumLowPointValues(new int[][] { new int[] {2} }).Should().Be(3);
    }

    [Test]
    public void TwoValueMapLowestPointWins()
    {
        SumLowPointValues(new int[][] { new int[] {3,2} }).Should().Be(3);
    }

    [Test]
    public void TwoValueMapWithEqualLowPointsAreSummed()
    {
        SumLowPointValues(new int[][] { new int[] {2,2} }).Should().Be(6);
    }

    [Test]
    public void ThreeValueMapWithPeakSeparatingEqualMinima()
    {
        SumLowPointValues(new int[][] { new int[] {1,2,1} }).Should().Be(4);
    }

    [Test]
    public void TwoDimensionalMapWithPeaksSeparatingEqualMinima()
    {
        SumLowPointValues(new int[][] {
            new int[] {1,2,1},
            new int[] {2,1,2}
        }).Should().Be(6);
    }

    [Test]
    public void PointsInARowWithTwoMinima()
    {
        // Test to force summing of multiple levels of minima
        SumLowPointValues(new int[][] { new int[] {1,3,2} }).Should().Be(5);
        SumLowPointValues(new int[][] { new int[] {1,3,2,1} }).Should().Be(4);
    }

    [Test]
    public void PointsInAColumnWithTwoMinima()
    {
        SumLowPointValues(new int[][] {
            new int[] {1},
            new int[] {3},
            new int[] {2}
        }).Should().Be(5);
        SumLowPointValues(new int[][] {
            new int[] {1},
            new int[] {2},
            new int[] {3},
            new int[] {1}
        }).Should().Be(4);
    }

    [Test]
    public void PlateauAsAMinima()
    {
        SumLowPointValues(new int[][] { new int[] {1,3,2,2} }).Should().Be(8);
    }

    [Test]
    public void PlateauNotAsAMinima()
    {
        SumLowPointValues(new int[][] { new int[] {1,3,2,2,1} }).Should().Be(4);
    }

    [Test]
    public void TwoBasinsSeparatedByAWallOf9s()
    {
        ProductOf3BiggestBasins(new int[][] {
            new int[] {1,9,1},
            new int[] {2,9,2},
            new int[] {2,9,2}
        }).Should().Be(9);
    }

    [Test]
    public void ThreeBasinsSeparatedByWallsOf9s()
    {
        ProductOf3BiggestBasins(new int[][] {
            new int[] {1,9,2,9,1},
            new int[] {1,9,2,9,1},
            new int[] {1,9,2,9,1}
        }).Should().Be(27);
    }
    
    [Test]
    public void CalculateProductOfTheBiggest3Basins()
    {
        ProductOf3BiggestBasins(new int[][] {
            new int[] {1,9,2,9,1,9,1},
            new int[] {9,9,2,9,1,9,1},
            new int[] {9,9,2,9,1,9,1}
        }).Should().Be(27);
    }

    private int ProductOf3BiggestBasins(int[][] input)
    {
        var plateaus = new List<List<(int, int)>>();
        for(var currentLocalMinima = 0; currentLocalMinima < 9; currentLocalMinima++)
        {
            HashSet<(int, int)> pointsAtMinima = GetPointsAtMinima(input, currentLocalMinima);
            var plateau = AssemblePlateaus(pointsAtMinima);
            plateaus.AddRange(plateau);
        }
        var basins = AssembleBasins(plateaus).OrderByDescending(basin => basin.Count());
        var total = 1;
        foreach(var basin in basins.Take(3))
        {
            total *= basin.Count();
        }
        return total;
    }

    private List<List<(int, int)>> AssembleBasins(List<List<(int, int)>> plateaus)
    {
        var basins = new List<List<(int, int)>>();
        foreach(var plateau in plateaus)
        {
            var foundBasin = false;
            foreach(var basin in basins)
            {
                if(basin.Any(p => IsAdjacentToAny(p.Item1, p.Item2, plateau)))
                {
                    basin.AddRange(plateau);
                    foundBasin = true;
                    break;
                }
            }
            if(foundBasin == false) basins.Add(plateau);
        }
        return basins;
    }

    [Test]
    public void GoldenInputTestPart1()
    {
        var points = LoadGoldenInput();
        SumLowPointValues(points).Should().Be(518);
    }

    [Test]
    public void GoldenInputTestPart2()
    {
        var points = LoadGoldenInput();
        ProductOf3BiggestBasins(points).Should().Be(949905);
    }

    private int[][] LoadGoldenInput()
    {
        var lines = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        var points = new List<int[]>();
        foreach(var line in lines)
        {
            var row = line.Select(c => int.Parse(c.ToString())).ToArray();
            points.Add(row);
        }
        return points.ToArray();
    }

    private int SumLowPointValues(int[][] input)
    {
        if(input.Length == 0)
        {
            return 0;
        }
        var globalMinima = FindGlobalMinimumValue(input);
        var globalMaxima = FindGlobalMaximumValue(input);
        int total = 0;
        var alreadyCheckedPoints = new List<(int, int)>();
        for(var currentLocalMinima = globalMinima; currentLocalMinima <= globalMaxima; currentLocalMinima++)
        {
            HashSet<(int, int)> pointsAtMinima = GetPointsAtMinima(input, currentLocalMinima);
            var plateaus = AssemblePlateaus(pointsAtMinima);
            foreach (var plateau in plateaus)
            {
                if (IsMinima(plateau, alreadyCheckedPoints))
                {
                    total += plateau.Count() * (currentLocalMinima + 1);
                    // Console.WriteLine($"Found minima plateau for {currentLocalMinima} at:");
                    // foreach (var point in plateau) Console.WriteLine($"  {point.Item1},{point.Item2}");
                }
            }
            foreach (var plateau in plateaus)
            {
                alreadyCheckedPoints.AddRange(plateau);
            }
        }
        return total;
    }

    private static HashSet<(int, int)> GetPointsAtMinima(int[][] input, int currentLocalMinima)
    {
        var pointsAtMinima = new HashSet<(int, int)>();
        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == currentLocalMinima)
                {
                    pointsAtMinima.Add((row, col));
                }
            }
        }

        return pointsAtMinima;
    }

    private bool IsMinima(List<(int, int)> plateau, List<(int, int)> alreadyCheckedPoints)
    {
        return plateau.All(p => !IsAdjacentToAny(p.Item1, p.Item2, alreadyCheckedPoints));
    }

    private List<List<(int, int)>> AssemblePlateaus(HashSet<(int rowIndex, int colIndex)> pointsAtMinima)
    {
        var plateaus = new List<List<(int, int)>>();
        while(pointsAtMinima.Count() != 0)
        {
            var plateau = new List<(int, int)>();
            plateaus.Add(plateau);
            plateau.Add(pointsAtMinima.First());
            pointsAtMinima.Remove(pointsAtMinima.First());
            foreach(var point in pointsAtMinima)
            {
                if(IsAdjacentToAny(point.Item1, point.Item2, plateau))
                {
                    plateau.Add(point);
                    pointsAtMinima.Remove(point);
                }
            }
        }
        return plateaus;
    }

    private bool IsAdjacentToAny(int row, int col, List<(int, int)> points)
    {
        if( points.Contains((row,   col+1)) ||
            points.Contains((row,   col-1)) ||
            points.Contains((row+1, col))   ||
            points.Contains((row-1, col)))
        {
            return true;
        }
        return false;
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

    private int FindGlobalMaximumValue(int[][] input)
    {
        var globalMaximum = int.MinValue;
        foreach(var row in input)
        {
            var rowMax = row.Max();
            if(rowMax > globalMaximum) globalMaximum = rowMax;
        }
        return globalMaximum;
    }
}