namespace SharpTtsServiceProg.Workers.Fasades;

public class HeadersTextParser
{
    // private PromptBuilder GetBuilder((string Repo, string Loca) adrTuple, CultureInfo culture)
    // {
    //     var text = repoService.Methods.GetText2(adrTuple);
    //     var elements = operationsService.Header.Select2.GetElements(text);
    //     var builder = new PromptBuilder();
    //     builder.Culture = culture;
    //
    //     foreach (var elem in elements)
    //     {
    //         if (elem.Type == "Header")
    //         {
    //             HeaderParser(builder, elem.Text);
    //         }
    //
    //         if (elem.Type != "Header")
    //         {
    //             LineParser(builder, elem.Text);
    //         }
    //     }
    //
    //     return builder;
    // }
    //
    // private void HeaderParser(
    //     PromptBuilder builder,
    //     string line)
    // {
    //     line = AllReplacements(line);
    //     builder.AppendText(line);
    //     builder.AppendBreak();
    // }
    //
    // private void LineParser(
    //     PromptBuilder builder,
    //     string line)
    // {
    //     var version = "v;";
    //     if (line.StartsWith(version))
    //     {
    //         var start = version.Length;
    //         var stop = line.Length;
    //         var length = stop - start;
    //         line = line.Substring(start, length);
    //     }
    //
    //     var at = '@';
    //     if (line.StartsWith(at))
    //     {
    //         line = line.Trim(at);
    //     }
    //
    //     line = AllReplacements(line);
    //     line.Replace(" m2w ", " man to woman ");
    //
    //     builder.AppendText(line);
    //     builder.AppendBreak();
    // }

    private string AllReplacements(
        string line)
    {
        line = ReplaceShortcut(line, "m2w", "man to woman");
        return line;
    }

    private string ReplaceShortcut(
        string line,
        string shortcut,
        string replacement)
    {
        var space = ' ';
        if (line.Contains(shortcut))
        {
            var tmp01 = space + shortcut + space;
            if (line.Contains(tmp01))
            {
                line = line.Replace(tmp01, space + replacement + space);
            }

            var tmp02 = shortcut + space;
            if (line.StartsWith(tmp02))
            {
                line = line.Replace(tmp02, replacement + space);
            }

            var tmp03 = space + shortcut;
            if (line.EndsWith(tmp03))
            {
                line = line.Replace(tmp03, space + replacement);
            }
        }

        return line;
    }
}
