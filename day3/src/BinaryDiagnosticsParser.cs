using System;
using System.Collections.Generic;
using System.Linq;

namespace day3;

internal class BinaryDiagnosticsParser
{
    public Diagnostics Parse(string[] binaryDiagnostics)
    {
        if(binaryDiagnostics.Length <= 0)
        {
            return new Diagnostics();
        }

        var gammaRate = FindGammaRate(binaryDiagnostics);
        return new Diagnostics
        {
            GammaRate = gammaRate,
            EpsilonRate = FindEpsilonRate(gammaRate, binaryDiagnostics[0].Length),
            O2GeneratorRating = FindDiagnosticBasedOnFilter(binaryDiagnostics, FilterToMostCommonDigitAtEachIndex),
            CO2ScrubberRating = FindDiagnosticBasedOnFilter(binaryDiagnostics, FilterToLeastCommonDigitAtEachIndex)
        };
    }

    private long FindDiagnosticBasedOnFilter(
        string[] binaryDiagnostics,
        Func<string, DigitCount, int, bool> diagnosticFilter)
    {
        var maxIndex = binaryDiagnostics[0].Length - 1;
        var filteredDiagnostics = binaryDiagnostics.ToList();
        for (int index = 0; index <= maxIndex; index++)
        {
            var digitCountsForIndex = GetDigitCountForIndex(index, filteredDiagnostics);
            filteredDiagnostics = filteredDiagnostics
                .Where(diagnostic => diagnosticFilter(diagnostic, digitCountsForIndex, index))
                .ToList();
            if (filteredDiagnostics.Count() == 1)
            {
                break;
            }
        }
        if (filteredDiagnostics.Count() == 1)
        {
            return ConvertFromBinaryAsStringToInt(filteredDiagnostics.First());
        }
        return 0;
    }

    private static bool FilterToMostCommonDigitAtEachIndex(string diagnostic, DigitCount digitCountsForIndex, int index)
    {
        return diagnostic[index] == MostCommonDigit(digitCountsForIndex);
    }

    private static char MostCommonDigit(DigitCount digitCount)
    {
        return digitCount.ZeroCount > digitCount.OneCount ? '0' : '1';
    }

    private static bool FilterToLeastCommonDigitAtEachIndex(string diagnostic, DigitCount digitCountsForIndex, int index)
    {
        return diagnostic[index] == LeastCommonDigit(digitCountsForIndex);
    }

    private static char LeastCommonDigit(DigitCount digitCount)
    {
        if(digitCount.OneCount == 0) return '0';
        if(digitCount.ZeroCount == 0) return '1';
        return digitCount.OneCount < digitCount.ZeroCount ? '1' : '0';
    }

    private static DigitCount GetDigitCountForIndex(
        int index, List<string> binaryDiagnostics)
    {
        var digitCountAtIndex = new DigitCount();
        foreach (var diagnostic in binaryDiagnostics)
        {
            if (diagnostic[index] == '0') digitCountAtIndex.ZeroCount++;
            else if (diagnostic[index] == '1') digitCountAtIndex.OneCount++;
        }
        return digitCountAtIndex;
    }

    private class DigitCount
    {
        public int ZeroCount { get; set; }
        public int OneCount { get; set; }
    }

    private long FindEpsilonRate(long gammaRate, int numberOfBitsToCareAbout)
    {
        var maskForBitsWeCareAbout = (long)Math.Pow(2, numberOfBitsToCareAbout) - 1;
        return ~gammaRate & maskForBitsWeCareAbout;
    }

    private static long FindGammaRate(string[] binaryDiagnostics)
    {
        var mostCommonCharacters = "";
        if (binaryDiagnostics.Length == 1)
        {
            mostCommonCharacters = binaryDiagnostics[0];
        }
        else
        {
            var mostCommonCharactersSplit =
                CharacterFrequenciesOrderedByIndex(binaryDiagnostics)
                    .Select(characterFrequenciesAtIndex =>
                        characterFrequenciesAtIndex.Frequencies.Last().Character);
            mostCommonCharacters = string.Concat(mostCommonCharactersSplit);
        }
        return ConvertFromBinaryAsStringToInt(mostCommonCharacters);
    }

    private static long ConvertFromBinaryAsStringToInt(string binaryAsString)
    {
        return Convert.ToInt64(binaryAsString, 2);
    }

    private static IOrderedEnumerable<CharacterFrequenciesAtIndex> CharacterFrequenciesOrderedByIndex(string[] binaryDiagnostics)
    {
        var characterFrequenciesByIndex = new Dictionary<int, CharacterFrequencies>();
        foreach (var diagnostic in binaryDiagnostics)
        {
            for (int index = 0; index < diagnostic.Length; index++)
            {
                if(!characterFrequenciesByIndex.TryGetValue(index, out var characterFrequencies))
                {
                    characterFrequencies = new CharacterFrequencies();
                    characterFrequenciesByIndex.Add(index, characterFrequencies);
                }
                var character = diagnostic[index];
                if (!characterFrequencies.Frequencies.TryAdd(character, 1))
                {
                    characterFrequencies.Frequencies[character]++;
                }
            }
        }

        return characterFrequenciesByIndex
            .Select(kvp => new CharacterFrequenciesAtIndex
            {
                Index = kvp.Key,
                Frequencies = kvp.Value.Frequencies
                    .Select(kvp => new CharacterFrequency { Character = kvp.Key, Frequency = kvp.Value })
                    .OrderBy(charFrequency => charFrequency.Frequency)
            })
            .OrderBy(a => a.Index);
    }

    private class CharacterFrequencies
    {
        public Dictionary<char, int> Frequencies { get; } = new Dictionary<char, int>();
    }
}