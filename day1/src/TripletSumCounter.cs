
using System.Collections.Generic;

namespace day1;

public class TripletSumCounter
{
    public int CountConsecutiveIncreases(int[] measurements)
    {
        return new ConsecutiveIncreaseCounter().Count(AsSummedTriplets(measurements));
    }

    private IEnumerable<(int first, int second)> AsSummedTriplets(int[] input)
    {
        const int MinimumNumberOfMeasurements = 4;
        for(int i = 0; i <= input.Length - MinimumNumberOfMeasurements; ++i)
        {
            var firstTriplet = input[i] + input[i+1] + input[i+2];
            var secondTriplet = input[i+1] + input[i+2] + input[i+3];
            yield return (firstTriplet, secondTriplet);
        }
    }
}