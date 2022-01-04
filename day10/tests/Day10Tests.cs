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
        AnalyseChunks("").FailedSyntaxPosition.Should().Be(NoSyntaxErrors);
    }

    [TestCase("}")]
    [TestCase(")")]
    [TestCase("]")]
    [TestCase(">")]
    public void LoneChunkEndIsFailure(string chunkEnd)
    {
        AnalyseChunks(chunkEnd).FailedSyntaxPosition.Should().Be(0);
    }

    [TestCase("{")]
    [TestCase("(")]
    [TestCase("[")]
    [TestCase("<")]
    public void LoneOpeningBraceIsIncomplete(string chunkStart)
    {
        AnalyseChunks(chunkStart).FailedSyntaxPosition.Should().Be(NoSyntaxErrors);
    }

    [TestCase("{}")]
    [TestCase("[]")]
    [TestCase("()")]
    [TestCase("<>")]
    public void PairedBracesIsComplete(string input)
    {
        AnalyseChunks(input).FailedSyntaxPosition.Should().Be(NoSyntaxErrors);
    }

    [TestCase("{]", 1)]
    [TestCase("{{}]", 3)]
    [TestCase("{{}[]]", 5)]
    public void UnmatchedChunkEndIsFailure(string input, int failurePosition)
    {
        AnalyseChunks(input).FailedSyntaxPosition.Should().Be(failurePosition);
        AnalyseChunks("{{}]").FailedSyntaxPosition.Should().Be(3);
        AnalyseChunks("{{}[]]").FailedSyntaxPosition.Should().Be(5);
    }

    [TestCase("[({(<(())[]>[[{[]{<()<>>", "}}]])})]")]
    [TestCase("[(()[<>])]({[<{<<[]>>(", ")}>]})")]
    public void ExpectedClosingTokensAreCorrect(string input, string expectedClosingTokens)
    {
        string.Join("", AnalyseChunks(input).ExpectedClosingTokens).Should().Be(expectedClosingTokens);
    }

    [TestCase("])}>", 294)]
    [TestCase("}}]])})]", 288957)]
    [TestCase(")}>]})", 5566)]
    public void CalculateScores(string input, long expectedScore)
    {
        CalculateScore(input).Should().Be(expectedScore);
    }

    private long CalculateScore(string input)
    {
        long total = 0;
        foreach(var c in input)
        {
            total *= 5;
            total += ValueOf(c);
        }
        return total;
    }

    private int ValueOf(char c)
    {
        if(c == ')') return 1;
        if(c == ']') return 2;
        if(c == '}') return 3;
        if(c == '>') return 4;
        return 0;
    }

    [Test]
    public void GoldenInputTestPart1()
    {
        var lines = LoadGoldenInput();
        int score = 0;
        foreach(var line in lines)
        {
            int failurePosition = AnalyseChunks(line).FailedSyntaxPosition;
            if(failurePosition != NoSyntaxErrors)
            {
                score += GetValue(line[failurePosition]);
            }
        }
        score.Should().Be(299793);
    }

    [Test]
    public void GoldenInputTestPart2()
    {
        var lines = LoadGoldenInput();
        var scores = new List<long>();
        foreach(var line in lines)
        {
            var result = AnalyseChunks(line);
            if(result.FailedSyntaxPosition == NoSyntaxErrors)
            {
                scores.Add(CalculateScore(string.Join("", result.ExpectedClosingTokens)));
            }
        }
        scores.Sort();
        var middleIndex = scores.Count / 2;
        scores[middleIndex].Should().Be(3654963618);
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

    AnalysisResult AnalyseChunks(string input)
    {
        var expectedClosingTokens = new Stack<char>();
        for(var index = 0; index < input.Length; index++)
        {
            if(IsClosingToken(input[index]))
            {
                if(expectedClosingTokens.Count() == 0)
                {
                    return new AnalysisResult(index, expectedClosingTokens);
                }
                else if(input[index] != expectedClosingTokens.Peek())
                {
                    return new AnalysisResult(index, expectedClosingTokens);
                }
                expectedClosingTokens.Pop();
            }
            else if(IsOpeningToken(input[index]))
            {
                expectedClosingTokens.Push(FindClosingToken(input[index]));
            }
        }
        return new AnalysisResult(NoSyntaxErrors, expectedClosingTokens);
    }

    internal class AnalysisResult
    {
        public AnalysisResult(int failedSyntaxPosition, Stack<char> expectedClosingTokens)
        {
            FailedSyntaxPosition = failedSyntaxPosition;
            ExpectedClosingTokens = expectedClosingTokens;
        }

        public readonly int FailedSyntaxPosition = NoSyntaxErrors;
        public readonly Stack<char> ExpectedClosingTokens;
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