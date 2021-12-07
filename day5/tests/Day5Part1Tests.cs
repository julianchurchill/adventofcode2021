using NUnit.Framework;
using FluentAssertions;
using System.Linq;
using System;

namespace day5.tests;

public class Day5Part1Tests
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
                new Line(0, 0, 0, 0),
                new Line(0, 0, 0, 0)})
            .Should().Be(1);
    }

    [Test]
    public void TwoOnePointLinesNotOverlapping()
    {
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 0, 0),
                new Line(1, 0, 1, 0)})
            .Should().Be(0);
    }

    [Test]
    public void MatchingLinesOverlapByLengthOfLine()
    {
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 1, 0),
                new Line(0, 0, 1, 0)})
            .Should().Be(2);
    }

    [Test]
    public void TwoLinesInSamePlaneWithOneTotalOverlap()
    {
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 0, 0),
                new Line(0, 0, 1, 0)})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 1, 0),
                new Line(0, 0, 0, 0)})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 0, 0),
                new Line(0, 0, 0, 1)})
            .Should().Be(1);
        CountOverlappingPoints(new Line[] { 
                new Line(0, 0, 0, 0),
                new Line(1, 0, 0, 0)})
            .Should().Be(1);
    }

    // [Test]
    // public void TwoLinesInSamePlaneWithPartialOverlap()
    // {
    //     CountOverlappingPoints(new Line[] { 
    //             new Line(0, 0, 1, 0),
    //             new Line(1, 0, 2, 0)})
    //         .Should().Be(1);
    // }

    private int CountOverlappingPoints(Line[] lines)
    {
        if(lines.Count() == 0)
        {
            return 0;
        }

        var line1 = lines[0];
        var line2 = lines[1];
        if(line1.Equals(line2))
        {
            return line1.Length;
        }

        // Same single coord means overlap size is smallest line
        if(line1.First.Equals(line2.First) || line1.Second.Equals(line2.Second))
        {
            return Math.Min(line1.Length, line2.Length);
        }

        return 0;
    }

    public class Line
    {
        public Line(int x1, int y1, int x2, int y2)
        {
            First = new Point(x1, y1);
            Second = new Point(x2, y2);
        }

        public Point First { get; init; }
        public Point Second { get; init; }

        public int Length
        { 
            get
            {
                var xDiff = Second.X - First.X;
                var yDiff = Second.Y - First.Y;
                // add 1 to give the line some length
                // this is added as coords represent a point of length 1
                // e.g. (0,0 -> 0,0) means length == 1, not length 0
                return (int)Math.Sqrt(xDiff * xDiff + yDiff * yDiff) + 1;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return obj is Line p && (First, Second).Equals((p.First, p.Second));
        }
        
        public override int GetHashCode()
        {
            return (First, Second).GetHashCode();
        }
    }

    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; init; }
        public int Y { get; init; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return obj is Point p && (X, Y).Equals((p.X, p.Y));
        }
        
        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }

    // [Test]
    // public void GoldenTestFirstWinningBingoBoard()
    // {
    //     (var numbers, var boards) = LoadGoldenInput();

    //     var result = FindFirstWinningBingoBoard(
    //         boards.ToArray(),
    //         numbers);
    //     result.Winner.Should().BeTrue();
    //     result.CalculateScore().Should().Be(87456);
    // }
}