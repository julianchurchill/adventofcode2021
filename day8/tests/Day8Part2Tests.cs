using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace day8;

public class Day8Part2Tests
{
    [Test]
    public void DeciperAll1s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |ab ab ab ab")
            .Should().Be(1111);
    }

    [Test]
    public void DeciperAll7s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |dab dab dab dab")
            .Should().Be(7777);
    }

    [Test]
    public void DeciperAll4s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |eafb eafb eafb eafb")
            .Should().Be(4444);
    }

    [Test]
    public void DeciperAll8s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |acedgfb acedgfb acedgfb acedgfb")
            .Should().Be(8888);
    }

    [Test]
    public void DeciperAll5s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |cdfbe cdfbe cdfbe cdfbe")
            .Should().Be(5555);
    }

    [Test]
    public void DeciperAll2s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |gcdfa gcdfa gcdfa gcdfa")
            .Should().Be(2222);
    }

    [Test]
    public void DeciperAll3s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |fbcad fbcad fbcad fbcad")
            .Should().Be(3333);
    }

    [Test]
    public void DeciperAll9s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |cefabd cefabd cefabd cefabd")
            .Should().Be(9999);
    }

    [Test]
    public void DeciperAll6s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |cdfgeb cdfgeb cdfgeb cdfgeb")
            .Should().Be(6666);
    }

    [Test]
    public void DeciperAll0s()
    {
        DecipherDigits("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |cagedb cagedb cagedb cagedb")
            .Should().Be(0000);
    }

    [Test]
    public void GoldenInput()
    {
        var lines = LoadGoldenInput();
        int count = 0;
        foreach(var line in lines)
        {
            count += DecipherDigits(line);
        }
        count.Should().Be(1046281);
    }

    private string[] LoadGoldenInput()
    {
        return System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
    }

    private int DecipherDigits(string input)
    {
        var splitInput = input.Split("|");
        var uniqueSignalPatterns = splitInput[0].Split(" ");
        var one = uniqueSignalPatterns.Single(pattern => pattern.Length == 2);
        var four = uniqueSignalPatterns.Single(pattern => pattern.Length == 4);
        var digits = splitInput[1].Split(" ");
        var output = "";
        foreach(var digit in digits)
        {
            if(digit.Length == 2) output += "1";
            else if(digit.Length == 3) output += "7";
            else if(digit.Length == 4) output += "4";
            else if(digit.Length == 7) output += "8";
            else if(digit.Length == 5)
            {
                if(digit.Shares(2).SegmentsWith(four)) output += "2";
                else if(digit.Shares(2).SegmentsWith(one)) output += "3";
                else output += "5";
            }
            else if(digit.Length == 6)
            {
                if(digit.Shares(4).SegmentsWith(four)) output += "9";
                else if(digit.Shares(2).SegmentsWith(one)) output += "0";
                else output += "6";
            }
        }
        return int.Parse(output);
    }
}

internal static class StringExtensions
{
    public static InputAndSegmentCount Shares(this string input, int segmentCount)
    {
        return new InputAndSegmentCount(input, segmentCount);
    }
}

internal static class InputAndSegmentCountExtensions
{
    public static bool SegmentsWith(this InputAndSegmentCount inputAndSegmentCount, string digit)
    {
        return inputAndSegmentCount.Input.Intersect(digit).Count() == inputAndSegmentCount.SegmentCount;
    }
}

internal class InputAndSegmentCount
{
    public InputAndSegmentCount(string input, int segmentCount)
    {
        this.Input = input;
        this.SegmentCount = segmentCount;
    }

    public string Input { get; }
    public int SegmentCount { get; }
}