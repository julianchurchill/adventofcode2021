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
        var maskForBitsWeCareAbout = (long)Math.Pow(2, binaryDiagnostics[0].Length) - 1;
        return new Diagnostics
        {
            GammaRate = gammaRate,
            EpsilonRate = ~gammaRate & maskForBitsWeCareAbout
        };
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