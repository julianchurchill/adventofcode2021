using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace day4.tests;

public class Day4Tests
{
    [Test]
    public void NoBoardsMeansNoWinner()
    {
        FindFirstWinningBingoBoard(new Board[] {}, new int[] { 1, 2, 3 })
            .Winner.Should().BeFalse();
    }

    [Test]
    public void NoNumbersMeansNoWinner()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        FindFirstWinningBingoBoard(new Board[] { board1To25 }, new int[] {})
            .Winner.Should().BeFalse();
    }

    [Test]
    public void OneBoardWithNoMatchingNumbersDoesNotWin()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 26, 27, 28, 29, 30 })
            .Winner.Should().BeFalse();
    }

    [Test]
    public void OneBoardTopRowWinsAfter5Numbers()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 1, 2, 3, 4, 5 });
        result.OriginalBoard.Should().Be(board1To25);
        result.Winner.Should().BeTrue();
    }

    [Test]
    public void OneBoardSecondRowWinsAfter5Numbers()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 6, 7, 8, 9, 10 });
        result.OriginalBoard.Should().Be(board1To25);
        result.Winner.Should().BeTrue();
    }

    [Test]
    public void OneBoardFirstColumnWinsAfter5Numbers()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 1, 6, 11, 16, 21 });
        result.OriginalBoard.Should().Be(board1To25);
        result.Winner.Should().BeTrue();
    }

    [Test]
    public void OneBoardSecondColumnWinsAfter5Numbers()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 2, 7, 12, 17, 22 });
        result.OriginalBoard.Should().Be(board1To25);
        result.Winner.Should().BeTrue();
    }

    [Test]
    public void SecondBoardSecondColumnWinsAfter5Numbers()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var board26To50 = new Board(CreateIntSquare(26, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board26To50, board1To25 },
                new int[] { 2, 7, 12, 17, 22 });
        result.OriginalBoard.Should().Be(board1To25);
        result.Winner.Should().BeTrue();
    }

    [Test]
    public void ScoreAWinningBoard()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var result = FindFirstWinningBingoBoard(
                new Board[] { board1To25 },
                new int[] { 1, 2, 3, 4, 5 });
        var unmarkedSum = Enumerable.Range(6, 20).Aggregate(0, (total, next) => total = total + next);
        result.CalculateScore().Should().Be(unmarkedSum*5);
    }


    [Test]
    public void FindLastWinningBoard()
    {
        var board1To25 = new Board(CreateIntSquare(1, 5));
        var board26To50 = new Board(CreateIntSquare(2, 5));
        var result = FindLastWinningBingoBoard(
                new Board[] { board26To50, board1To25 },
                new int[] { 1, 2, 3, 4, 5, 6 });
        result.OriginalBoard.Should().Be(board26To50);
        result.Winner.Should().BeTrue();
    }

    private List<List<int>> CreateIntSquare(int start, int rowLength)
    {
        var intSquare = new List<List<int>>();
        for(int next = start; next < start+rowLength*rowLength; next+=rowLength)
        {
            intSquare.Add(Enumerable.Range(next, rowLength).ToList());
        }
        return intSquare;
    }

    private MarkedBoard FindLastWinningBingoBoard(Board[] boards, int[] numbers)
    {
        if(boards.Length == 0 || numbers.Length == 0)
        {
            return new MarkedBoard(new Board(new List<List<int>>()));
        }

        var markedBoards = boards.Select(b => new MarkedBoard(b)).ToList();

        MarkedBoard lastWinner = null;
        foreach(var number in numbers)
        {
            foreach(var markedBoard in markedBoards)
            {
                markedBoard.UpdateMarkedNumbers(number);
                var previousWinState = markedBoard.Winner;
                markedBoard.AssessWinState();
                if(previousWinState == false && markedBoard.Winner) lastWinner = markedBoard;
            }
            if(markedBoards.All(m => m.Winner)) break;
        }

        return lastWinner;
    }

    private MarkedBoard FindFirstWinningBingoBoard(Board[] boards, int[] numbers)
    {
        if(boards.Length == 0 || numbers.Length == 0)
        {
            return new MarkedBoard(new Board(new List<List<int>>()));
        }

        var markedBoards = boards.Select(b => new MarkedBoard(b)).ToList();

        foreach(var number in numbers)
        {
            foreach(var markedBoard in markedBoards)
            {
                markedBoard.UpdateMarkedNumbers(number);
                markedBoard.AssessWinState();
                if(markedBoard.Winner) return markedBoard;
            }
        }

        return new MarkedBoard(boards[0]);
    }

    internal class Board
    {
        public Board(List<List<int>> values)
        {
            Values = values;
        }
        public List<List<int>> Values { get; init; }
    }

    internal class MarkedBoard
    {
        public MarkedBoard(Board originalBoard)
        {
            OriginalBoard = originalBoard;
            MarkedNumbers = CreateMarkedNumbers();
        }

        private List<List<bool>> CreateMarkedNumbers()
        {
            var l = new List<List<bool>>();
            for(var i = 0; i < OriginalBoard.Values.Count(); i++)
            {
                l.Add(new List<bool>());
                for(var j = 0; j < OriginalBoard.Values[i].Count(); j++)
                {
                    l.Last().Add(false);
                }
            }
            return l;
        }

        public Board OriginalBoard { get; init; }

        public List<List<bool>> MarkedNumbers { get; }

        public bool Winner { get; set; } = false;

        private int LastNumberUpdated = 0;

        public int CalculateScore()
        {
            int score = 0;
            for(var i = 0; i < OriginalBoard.Values.Count(); i++)
            {
                for(var j = 0; j < OriginalBoard.Values[i].Count(); j++)
                {
                    if(MarkedNumbers[i][j] == false)
                    {
                        score += OriginalBoard.Values[i][j];
                    }
                }
            }
            score *= LastNumberUpdated;
            return score;
        }

        public void UpdateMarkedNumbers(int number)
        {
            LastNumberUpdated = number;
            for(var i = 0; i < OriginalBoard.Values.Count(); i++)
            {
                for(var j = 0; j < OriginalBoard.Values[i].Count(); j++)
                {
                    if(OriginalBoard.Values[i][j] == number)
                    {
                        MarkedNumbers[i][j] = true;
                    }
                }
            }
        }

        public void AssessWinState()
        {
            // Check the rows
            if(MarkedNumbers.Any(row => row.All(value => value == true)))
            {
                Winner = true;
            }

            // Check the columns
            for(var colIndex = 0; colIndex < MarkedNumbers[0].Count(); colIndex++)
            {
                if(MarkedNumbers.All(row => row[colIndex] == true))
                {
                    Winner = true;
                }
            }
        }
    }

    [Test]
    public void GoldenTestFirstWinningBingoBoard()
    {
        (var numbers, var boards) = LoadGoldenInput();

        var result = FindFirstWinningBingoBoard(
            boards.ToArray(),
            numbers);
        result.Winner.Should().BeTrue();
        result.CalculateScore().Should().Be(87456);
    }

    
    [Test]
    public void GoldenTestLastWinningBingoBoard()
    {
        (var numbers, var boards) = LoadGoldenInput();

        var result = FindLastWinningBingoBoard(
            boards.ToArray(),
            numbers);
        result.Winner.Should().BeTrue();
        result.CalculateScore().Should().Be(15561);
    }

    private (int[], Board[]) LoadGoldenInput()
    {
        var input = System.IO.File.ReadAllLines("../../../tests/goldeninput.txt");

        var numbers = input.First().Split(",").Select(t => int.Parse(t)).ToArray();

        var boardLines = input.Skip(1).Where(line => line.Trim() != string.Empty).ToList();
        var boards = new List<Board>();

        while(boardLines.Count() > 0)
        {
            var values = boardLines.Take(5)
                .Select(boardLine => new List<int>(
                    boardLine.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => int.Parse(s.Trim()))));
            boards.Add(new Board(values.ToList()));
            boardLines = boardLines.Skip(5).ToList();
        }
        return (numbers, boards.ToArray());
    }
}