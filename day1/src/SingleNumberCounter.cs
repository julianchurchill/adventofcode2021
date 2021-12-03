
using System.Collections.Generic;

namespace day1;

public class SingleNumberCounter
{   
    public int CountConsecutiveIncreases(int[] measurements)
    {
        return new ConsecutiveIncreaseCounter().Count(AsPairs(measurements));
    }

    private IEnumerable<(int first, int second)> AsPairs(int[] input)
    {
        const int MinimumNumberOfMeasurements = 2;
        for(int i = 0; i <= input.Length - MinimumNumberOfMeasurements; ++i)
        {
            yield return (input[i], input[i+1]);
        }
    }
}
