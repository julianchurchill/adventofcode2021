using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace day8;

public class Day8Part1Tests
{
    [Test]
    public void No1478s()
    {
        Count1478s("be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb |fdgacbe cefdb cefbgd gcbe")
            .Should().Be(2);
        Count1478s("edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec |fcgedb cgb dgebacf gc")
            .Should().Be(3);
    }

    [Test]
    public void GoldenInput()
    {
        var lines = LoadGoldenInput();
        int count = 0;
        foreach(var line in lines)
        {
            count += Count1478s(line);
        }
        count.Should().Be(440);
    }

    private string[] LoadGoldenInput()
    {
        return System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
    }

    private int Count1478s(string input)
    {
        var digits = input.Split("|")[1].Split(" ");
        return digits.Count(digit => digit.Length == 2 || digit.Length == 3 || digit.Length == 4 || digit.Length == 7);
    }
}