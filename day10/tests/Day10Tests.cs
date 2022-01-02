using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace day10;

public class Tests
{
    [Test]
    public void NoFailuresIsMinus1()
    {
        FindFailedSyntaxPosition("").Should().Be(NoSyntaxErrors);
    }

    [TestCase("}")]
    [TestCase(")")]
    [TestCase("]")]
    [TestCase(">")]
    public void LoneChunkEndIsFailure(string chunkEnd)
    {
        FindFailedSyntaxPosition(chunkEnd).Should().Be(0);
    }

    [TestCase("{")]
    [TestCase("(")]
    [TestCase("[")]
    [TestCase("<")]
    public void LoneOpeningBraceIsIncomplete(string chunkStart)
    {
        FindFailedSyntaxPosition(chunkStart).Should().Be(NoSyntaxErrors);
    }

    [TestCase("{}")]
    [TestCase("[]")]
    [TestCase("()")]
    [TestCase("<>")]
    public void PairedBracesIsComplete(string input)
    {
        FindFailedSyntaxPosition(input).Should().Be(NoSyntaxErrors);
    }

    [TestCase("{]", 1)]
    [TestCase("{{}]", 3)]
    [TestCase("{{}[]]", 5)]
    public void UnmatchedChunkEndIsFailure(string input, int failurePosition)
    {
        FindFailedSyntaxPosition(input).Should().Be(failurePosition);
        FindFailedSyntaxPosition("{{}]").Should().Be(3);
        FindFailedSyntaxPosition("{{}[]]").Should().Be(5);
    }

    [Test]
    public void GoldenInputTestPart1()
    {
        var lines = LoadGoldenInput();
        int score = 0;
        foreach(var line in lines)
        {
            int failurePosition = FindFailedSyntaxPosition(line);
            if(failurePosition != NoSyntaxErrors)
            {
                score += GetValue(line[failurePosition]);
            }
        }
        score.Should().Be(299793);
    }

    private int GetValue(char v)
    {
        if(v == ')') return 3;
        if(v == ']') return 57;
        if(v == '}') return 1197;
        if(v == '>') return 25137;
        return 0;
    }

    private string[] LoadGoldenInput()
    {
        return System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");
    }

    const int NoSyntaxErrors = -1;

    int FindFailedSyntaxPosition(string input)
    {
        var expectedClosingTokens = new Stack<char>();
        for(var index = 0; index < input.Length; index++)
        {
            if(IsClosingToken(input[index]))
            {
                if(expectedClosingTokens.Count() == 0)
                {
                    return index;
                }
                else if(input[index] != expectedClosingTokens.Peek())
                {
                    return index;
                }
                expectedClosingTokens.Pop();
            }
            else if(IsOpeningToken(input[index]))
            {
                expectedClosingTokens.Push(FindClosingToken(input[index]));
            }
        }
        return NoSyntaxErrors;
    }

    private char FindClosingToken(char token)
    {
        if(token == '{') return '}';
        if(token == '[') return ']';
        if(token == '(') return ')';
        if(token == '<') return '>';
        return 'x';
    }

    private static bool IsOpeningToken(char token)
    {
        return
            token == '{' ||
            token == '[' ||
            token == '(' ||
            token == '<';
    }

    private static bool IsClosingToken(char token)
    {
        return
            token == '}' ||
            token == ']' ||
            token == ')' ||
            token == '>';
    }
}