using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace day5.tests;

public class Day5Tests
{
    [Test]
    public void NoLinesOverlaps0Points()
    {
        CountOverlappingPoints(new Line[] {}).Should().Be(0);
    }

    [Test]
    public void TwoOnePointLinesOverlap1Point()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (0, 0)),
                new Line((0, 0), (0, 0))})
            .Should().Be(1);
    }

    [Test]
    public void TwoOnePointLinesNotOverlapping()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (0, 0)),
                new Line((1, 0), (1, 0))})
            .Should().Be(0);
    }

    [Test]
    public void MatchingLinesOverlapByLengthOfLine()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (1, 0)),
                new Line((0, 0), (1, 0))})
            .Should().Be(2);
    }

    [Test]
    public void TwoLinesInSamePlaneWithOneTotalOverlap()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (0, 0)),
                new Line((0, 0), (1, 0))})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (1, 0)),
                new Line((0, 0), (0, 0))})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (0, 0)),
                new Line((0, 0), (0, 1))})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (0, 0)),
                new Line((1, 0), (0, 0))})
            .Should().Be(1);
    }

    [Test]
    public void TwoLinesInSamePlaneWithPartialOverlap()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (1, 0)),
                new Line((1, 0), (2, 0))})
            .Should().Be(1);
    }

    [Test]
    public void TwoThreePointLinesOverlap1Point()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (2, 0)),
                new Line((2, 0), (4, 0))})
            .Should().Be(1);
    }

    [Test]
    public void DiagonalLinesCanOverlap()
    {
        CountOverlappingPoints(new Line[] { 
                new Line((0, 0), (2, 2)),
                new Line((2, 0), (0, 2))})
            .Should().Be(1);
    }

    private int CountOverlappingPoints(Line[] lines)
    {
        var countOfPoints = new Dictionary<(int, int), int>();
        foreach (var line in lines)
        {
            InterpolateBetweenPointsInOnePlane(line.First, line.Second)
                .ForEach(p => AddPoint(countOfPoints, p));
        }
        return countOfPoints.Where(kvp => kvp.Value > 1).Count();
    }

    private static List<(int, int)> InterpolateBetweenPointsInOnePlane((int, int) start, (int, int) end)
    {
        if(start == end) return new List<(int, int)> { start };

        List<(int, int)> points = new List<(int, int)> { start };

        var x = start.Item1;
        var y = start.Item2;
        (int incrementX, int incrementY) = FindDeltaInOnePlane(start, end);
        do
        {
            x += incrementX;
            y += incrementY;
            points.Add((x, y));
        } while((x, y) != end);

        return points;
    }

    private static (int, int) FindDeltaInOnePlane((int, int) start, (int, int) end)
    {
        var incrementX = 0;
        var incrementY = 0;

        if(start.Item1 == end.Item1) // x coords are the same
        {
            incrementY = start.Item2 < end.Item2 ? 1 : -1;
        }
        else if(start.Item2 == end.Item2) // y coords are the same
        {
            incrementX = start.Item1 < end.Item1 ? 1 : -1;
        }
        else
        {
            incrementX = start.Item1 < end.Item1 ? 1 : -1;
            incrementY = start.Item2 < end.Item2 ? 1 : -1;
        }
        return (incrementX, incrementY);
    }

    private static void AddPoint(Dictionary<(int, int), int> countOfPoints, (int, int) point)
    {
        if (countOfPoints.ContainsKey(point))
        {
            countOfPoints[point]++;
        }
        else
        {
            countOfPoints.Add(point, 1);
        }
    }

    public class Line
    {
        public (int, int) First { get; init; }
        public (int, int) Second { get; init; }

        public Line((int, int) first, (int, int) second)
        {
            First = first;
            Second = second;
        }
    }

    [Test]
    public void GoldenTestFirstWinningBingoBoard()
    {
        var lines = LoadGoldenInput();
        var result = CountOverlappingPoints(lines);
        result.Should().Be(23864);
    }

    private Line[] LoadGoldenInput()
    {
        var input = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");

        var lines = new List<Line>();
        foreach(var line in input)
        {
            var coords = line.Split(" -> ");
            lines.Add(new Line(TextToCoord(coords[0]), TextToCoord(coords[1])));
        }
        return lines.ToArray();
    }

    private (int, int) TextToCoord(string coord)
    {
        var coords = coord.Split(",");
        return (int.Parse(coords[0]), int.Parse(coords[1]));
    }
}