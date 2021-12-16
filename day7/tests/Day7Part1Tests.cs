using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace day7;

public class Day7Part1Tests
{
    [Test]
    public void NoCrabsIsFreeToAlign()
    {
        FindCostToAlignCrabs(new int[] {}).Should().Be(0);
    }

    [Test]
    public void OneCrabIsFreeToAlign()
    {
        FindCostToAlignCrabs(new int[] {7 }).Should().Be(0);
    }

    [Test]
    public void TwoCrabsAlreadyAlignedAreFreeToAlign()
    {
        FindCostToAlignCrabs(new int[] {7, 7}).Should().Be(0);
    }

    [Test]
    public void TwoCrabsCostIsDistanceBetweenThem()
    {
        FindCostToAlignCrabs(new int[] {7, 9}).Should().Be(2);
        FindCostToAlignCrabs(new int[] {9, 7}).Should().Be(2);
    }

    [Test]
    public void ThreeCrabsCostIsDistanceBetweenFurthestPoints()
    {
        FindCostToAlignCrabs(new int[] {7, 9, 10}).Should().Be(3);
        FindCostToAlignCrabs(new int[] {5, 9, 10}).Should().Be(5);
        FindCostToAlignCrabs(new int[] {10, 9, 5}).Should().Be(5);
        FindCostToAlignCrabs(new int[] {9, 10, 5}).Should().Be(5);
    }

    [Test]
    public void FourUniqueCrabsCost()
    {
        FindCostToAlignCrabs(new int[] {7, 9, 10, 12}).Should().Be(6);
        FindCostToAlignCrabs(new int[] {6, 9, 10, 12}).Should().Be(7);
        FindCostToAlignCrabs(new int[] {1, 9, 10, 12}).Should().Be(12);
    }

    [Test]
    public void FourNonUniqueCrabsCost()
    {
        FindCostToAlignCrabs(new int[] {9, 9, 10, 12}).Should().Be(4);
        FindCostToAlignCrabs(new int[] {9, 9, 10, 10}).Should().Be(2);
        FindCostToAlignCrabs(new int[] {9, 9, 9, 10}).Should().Be(1);
        FindCostToAlignCrabs(new int[] {8, 9, 9, 10}).Should().Be(2);
    }
    
    [Test]
    public void FiveUniqueCrabsCost()
    {
        FindCostToAlignCrabs(new int[] {7, 9, 10, 12, 13}).Should().Be(9);
        FindCostToAlignCrabs(new int[] {6, 9, 10, 12, 13}).Should().Be(10);
        FindCostToAlignCrabs(new int[] {5, 9, 10, 12, 13}).Should().Be(11);
        FindCostToAlignCrabs(new int[] {4, 9, 10, 12, 13}).Should().Be(12);
        FindCostToAlignCrabs(new int[] {3, 9, 10, 12, 13}).Should().Be(13);
        FindCostToAlignCrabs(new int[] {2, 9, 10, 12, 13}).Should().Be(14);
        FindCostToAlignCrabs(new int[] {1, 9, 10, 12, 13}).Should().Be(15);
    }

    [Test]
    public void TenCrabsCost()
    {
        FindCostToAlignCrabs(new int[] {16,1,2,0,4,2,7,1,2,14}).Should().Be(37);
    }

    [Test]
    public void GoldenInput()
    {
        var crabs = LoadGoldenInput();
        FindCostToAlignCrabs(crabs).Should().Be(341558);
    }

    private int[] LoadGoldenInput()
    {
        var input = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
        return input[0].Split(",").Select(l => int.Parse(l)).ToArray();
    }

    private int FindCostToAlignCrabs(int[] crabs)
    {
        if(crabs.Length > 0 && crabs.Length <= 3)
        {
            return crabs.Max() - crabs.Min();
        }
        else if(crabs.Length >= 4)
        {
            var costs = new List<int>();
            foreach(var targetPosition in crabs.Distinct())
            {
                var cost = 0;
                foreach (var position in crabs)
                {
                    cost += Math.Abs(position - targetPosition);
                }
                costs.Add(cost);
            }
            return costs.Min();
        }

        return 0;
    }

    private static int AdjustPositionToNearestNumber(int[] crabs, int targetPosition)
    {
        return crabs.Aggregate((x, y) => Math.Abs(x - targetPosition) < Math.Abs(y - targetPosition) ? x : y);
    }
}