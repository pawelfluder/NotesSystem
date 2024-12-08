using System.Collections.Generic;
using System.Linq;

namespace SharpImageSplitterTests.Tests;

public static class TestsHelper
{
    public static List<string> ShowDiff(
        string expected,
        string actual)
    {
        List<string> diff;
        IEnumerable<string> set1 = expected.Split(' ').Distinct();
        IEnumerable<string> set2 = actual.Split(' ').Distinct();

        if (set2.Count() > set1.Count())
        {
            diff = set2.Except(set1).ToList();
        }
        else
        {
            diff = set1.Except(set2).ToList();
        }

        return diff;
    }
}
