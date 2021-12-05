using System.Linq;

namespace day3;

internal class CharacterFrequenciesAtIndex
{
    public int Index { get; init; }
    public IOrderedEnumerable<CharacterFrequency> Frequencies { get; init; }
}