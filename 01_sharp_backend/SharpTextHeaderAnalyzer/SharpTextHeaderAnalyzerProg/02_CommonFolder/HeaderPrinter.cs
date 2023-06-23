using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SharpYaml;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace TextHeaderAnalyzerFrameProj
{
    public class HeaderPrinter
    {
        private char tab = '\t';
        private string peak = @"//";
        private string newLine = Environment.NewLine;

        public void PrintToNewFile(YamlStream yamlStream2)
        {
           var nodes = yamlStream2.Documents[0].AllNodes;

         //var eventReader = new EventReader();

         var deserializer = new Deserializer();
         //deserializer.Deserialize

         var mappingNodes = nodes.Where(x => x is YamlMappingNode).Select(x => x as YamlMappingNode);

         foreach (var mappingNode in mappingNodes)
         {
            IDictionary<YamlNode, YamlNode> temp = mappingNode.Children;
            ICollection<YamlNode> keys = temp.Keys;
            ICollection<YamlNode> values = temp.Values;

            var type = values.ElementAt(0).NodeType;

            var gg = values.ElementAt(0) as YamlSequenceNode;

         }


         

         

         

           //var headers = nodes.Select(x => x.)
      }

      public void PrintToNewFile(string path, IEnumerable<Header> headers)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (File.Create(path))
            {
            }

            var fourLines = string.Concat(Enumerable.Repeat(newLine, 4));

            File.WriteAllText(path, fourLines + PrintToString(headers));
        }

        public void JoinPrintToFile(string path, IEnumerable<Header> headers)
        {

        }

        public string PrintToString(IEnumerable<Header> rootHeaders)
        {
            var output = new StringBuilder();

            var last = rootHeaders.Any() ? rootHeaders.Last() : null;
            foreach (var rootHeader in rootHeaders)
            {
                AddHeaderToStringBuilder(output, rootHeader, 0);
                if (rootHeader != last)
                {
                    output.Append(newLine);
                }
            }

            RemoveLastNewLine(output);

            return output.ToString();
        }

        private void RemoveLastNewLine(StringBuilder output)
        {
            while (IsLastLineNewLine(output))
            {
                output.Remove(output.Length - 2, 2);
            }
        }

        private bool IsLastLineNewLine(StringBuilder output)
        {
            var last = new string(new[] { output[output.Length - 2], output[output.Length - 1] });

            if (newLine == last)
            {
                return true;
            }

            return false;
        }

      private bool IsNewLineTwiceAtEnd(StringBuilder output)
        {
            var firstLast = new string(new[] { output[output.Length - 2], output[output.Length - 1] });
            var secondLast = new string(new[] { output[output.Length - 4], output[output.Length - 3] });

            if (firstLast == newLine && secondLast == newLine)
            {
                return true;
            }

            return false;
        }

        private void AddHeaderToStringBuilder(StringBuilder output, Header header, int level)
        {
            var headerBegin = new string(tab, level);
            var contentBegin = new string(tab, level + 1);

            output.Append(headerBegin + peak + header.Name + newLine);

            foreach (var contentLine in header.Content)
            {
                output.Append(contentBegin + contentLine + newLine);
            }

            var last = header.SubHeaders.Any() ? header.SubHeaders.Last() : null;
            foreach (var subHeader in header.SubHeaders)
            {
                AddHeaderToStringBuilder(output, subHeader as Header, level + 1);
                if (subHeader != last)
                {
                    output.Append(newLine);
                }
            }
        }
    }
}