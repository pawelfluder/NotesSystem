using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextHeaderAnalyzerCoreTestsProj
{
    public static class StringExtensions
    {
        public static string RemYamlBeginSpaces(this string input)
        {
            var newLine = "\r\n";
            var lines = input.Split(newLine);
            var cleanLines = lines.Select(x => x.TrimStart());
            var result = string.Join(newLine, cleanLines);

            return result;
        }
    }
}
