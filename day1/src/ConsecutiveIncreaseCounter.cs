
using System.Collections.Generic;
using System.Linq;

namespace day1;

public class ConsecutiveIncreaseCounter
{   
    public int Count(IEnumerable<(int first, int second)> values)
    {
        return values
            .Where(consecutiveValues => consecutiveValues.second > consecutiveValues.first)
            .Count();
    }
}