using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextHeaderAnalyzerCoreProj;

namespace TextHeaderAnalyzerFrameProj
{
    public partial class TextAnalyzer
    {
       private Translator translator = new Translator();

        public List<INotesContainer> AnalyzeFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            var subHeaders = FindHeaders(lines);
            return subHeaders;
        }

        public Header ToHeader(object obj)
        {
           return translator.ToHeader(obj);
        }

        public List<INotesContainer> FindHeaders(string[] lines)
        {
            // Create Notes Containers List
            var notesContainersList = new List<INotesContainer>();

            //Get header line number and level list
            var lineNumberAndLevelList = GetHeaderLines(lines);

            //ConvertAll() to header line number list
            var lineNumberList = lineNumberAndLevelList.ConvertAll(x => x.Item1);

            //Get Headers list
            var headers = GetHeaders(lines, lineNumberList);

            //Convert to level list
            var levelList = lineNumberAndLevelList.ConvertAll(x => x.Item2);

            //Correct first header level if postponed
            if (levelList.Any() && levelList.ElementAt(0) != 1)
            {
                levelList = CorrectHeaderLevelList(levelList);
            }

            //Convert to parent header number on list
            List<int> parentHeaderNumber = GetParentHeaderNumberList(levelList);

            //Add header to proper place and give level
            AddHeadersToParentHeader(headers, parentHeaderNumber);

            //Remove wrong headers on level 1
            RemoveWrongHeadersOnFirstLevel(headers, levelList);

            //
            notesContainersList.AddRange(headers);

            // Add lines between headers
            if (lineNumberAndLevelList.Count == 0)
            {
                var firstLines = GetFirstLines(lines.ToList());
                notesContainersList.Insert(0, firstLines);
            }
                

            return notesContainersList;
        }

        private NotesLines GetFirstLines(List<string> lines)
        {
            //var notesLines = new List<string>();
            var notesLines2 = new NotesLines(lines);

            return notesLines2;
        }

        private void RemoveWrongHeadersOnFirstLevel(List<Header> headers, List<int> levelList)
        {
            for (int i = levelList.Count-1; i >= 0; i--)
            {
                if (levelList[i] != 1)
                {
                    headers.RemoveAt(i);
                }
            }
        }

        private void AddHeadersToParentHeader(List<Header> headers, List<int> parentHeaderNumber)
        {
            var en = parentHeaderNumber.GetEnumerator();
            en.MoveNext();
            var i = 0;

            foreach (var header in headers)
            {
                var parentNumber = en.Current;
                if (parentNumber != 0)
                {
	                int number = ConvertToStartFromZero(parentNumber);
					var parent = headers.ElementAt(number);
                    parent.SubHeaders.Add(header);
                }

                en.MoveNext();
                i++;
            }
        }

        private int GetCountToFirstOne(List<int> list)
        {
            int i = 0;
            var en = list.GetEnumerator();

            while (en.MoveNext())
            {
                if (en.Current == 1)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public List<int> CorrectHeaderLevelList(List<int> levelList)
        {
            while (!levelList.Any(x => x == 1))
            {
                levelList = levelList.Select(x => x-1).ToList();
            }

            var count = GetCountToFirstOne(levelList);

            var part1 = Enumerable.Repeat(1, count);
            var part2 = levelList.Skip(count);

            return new List<int>(part1.Concat(part2));



            

            //var gg = levelList.First(x => x == 1);


            //var current = en.Current;

            //levelList.ForEach((x) =>
            //{
            //    if (current == 1)
            //    {
            //        break;
            //    }
            //});


            //var previousLevel = 0;


            //while (en.MoveNext())
            //{
            //    var current = en.Current;

            //    if (current == 1)
            //    {
            //        break;
            //    }

            //    en. = 1;
            //}

            //return levelList;

            //foreach (var level in levelList)
            //{
            //    if (previousLevel < level)
            //    {

            //    }

            //    previousLevel = level;
            //}
        }

        private bool NewMethod(ref List<int>.Enumerator en, out int next)//, out int next)
        {
            //current = en.Current;
            if (!en.MoveNext())
            {
                //current = 0;
                next = 0;
                return false;
            }
            next = en.Current;

            return true;
        }

        public List<int> GetParentHeaderNumberList(List<int> levelList)
        {
            var parentHeaderNumberList = new List<int>();
            var previousLevel = 0;
            
            var i = 1;

            foreach (var currentLevel in levelList)
            {
                if (currentLevel == 1)
                {
                    parentHeaderNumberList.Add(0);
                }
                else if (currentLevel > 1)
                {
                    if (previousLevel == currentLevel)
                    {
                        parentHeaderNumberList.Add(parentHeaderNumberList.Last());
                    }
                    else if (previousLevel < currentLevel)
                    {
                        parentHeaderNumberList.Add(i-1);
                    }
                    else
                    {
                       int z = 0;
                        var k = ConvertToStartFromZero(i - 1);
                       var searchBackLevel = levelList.ElementAt(k);
                        while (searchBackLevel > currentLevel)
                        {
                           searchBackLevel = levelList.ElementAt(k);
                            k--;
                           z++;
                        }

                        if (searchBackLevel == currentLevel)
                        {
                            var foundedIndex = ConvertToStartFromZero(i - z);
                            var parentIndex = parentHeaderNumberList.ElementAt(foundedIndex);

                            parentHeaderNumberList.Add(parentIndex);
                        }
                        else if (searchBackLevel < currentLevel)
                        {
                            var foundedIndex = ConvertToStartFromZero(i - z);
                            parentHeaderNumberList.Add(foundedIndex);
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }

                previousLevel = currentLevel;
                i++;
            }

            return parentHeaderNumberList;
        }


        private static List<(int, int)> GetHeaderLines(string[] lines)
        {
            //Todo write test for this method
            var headersNumbers = new List<(int, int)>();

            int lineNumber = 1;
            foreach (var line in lines)
            {
                var levelNumber = line.GetIsHeaderLevel();
                if (levelNumber > 0)
                {
                    headersNumbers.Add((lineNumber, levelNumber));
                }

                lineNumber++;
            }

            return headersNumbers;
        }

        public List<Header> GetHeaders(string[] lines, List<int> lineNumberList)
        {
            var headers = new List<Header>();
            lineNumberList.Add(lines.Count() + 1);

            var en = lineNumberList.GetEnumerator();
            en.MoveNext();

            while (GetCurrentAndNext(ref en, out var lineNumber, out var nextlineNumber))
            {
                string name = GetCorrectedHeaderName(lines, lineNumber);
                var content = GetContent(lines, lineNumber, nextlineNumber);
                var header = new Header(name, content);
                headers.Add(header);
            }

            return headers;
        }

        private void AddHeaderProperly(List<Header> subHeaders, List<int>.Enumerator en2)
        {
            for (int i = 0; i < subHeaders.Count; i++)
            {
                var level = en2.Current;
                

                en2.MoveNext();
            }
        }

        private bool GetCurrentAndNext(ref List<int>.Enumerator en, out int current, out int next)
        {
            current = en.Current;
            if (!en.MoveNext())
            {
                current = 0;
                next = 0;
                return false;
            }
            next = en.Current;

            return true;
        }

        private List<string> RemoveEmptyLinesFromEnd(List<string> content)
        {
            for (int i = content.Count - 1; i > 0; i--)
            {
                if (content[i] == String.Empty)
                {
                    content.Remove(content[i]);
                }
            }

            return content;
        }

        private List<string> GetContent(string[] lines, int lineNumber, int nextLineNumber)
        {
            int oneAfterHeaderLineNumber = lineNumber + 1;
            int number = ConvertToStartFromZero(oneAfterHeaderLineNumber);

            int numberOfContentLines = nextLineNumber - lineNumber - 1;

            
            var content = lines.SubArray(number, numberOfContentLines).ToList();

            var correctedContent = CorrectContent(content);
            correctedContent = RemoveEmptyLinesFromEnd(correctedContent);
            TrimStartEnd(ref correctedContent);

            return correctedContent;
        }

        private void TrimStartEnd(ref List<string> content)
        {
            for (int i = 0; i < content.Count; i++)
            {
                content[i] = content[i].Trim();
            }
        }

        private List<string> CorrectContent(List<string> content)
        {
            var result = content.Select(l => l.Replace("\t", "")).ToList();
            return result;
        }

        private string GetCorrectedHeaderName(string[] lines, int headerNumber)
        {
            int number = ConvertToStartFromZero(headerNumber);
            string headerName = lines[number];
            var result = headerName.TrimStart().Remove(0, 2);

            return result;
        }

        private static int gg(List<int> headersNumbers, int i)
        {

            return headersNumbers[i + 1];
        }

        private int ConvertToStartFromZero(int input)
        {
            return input - 1;
        }
    }
}