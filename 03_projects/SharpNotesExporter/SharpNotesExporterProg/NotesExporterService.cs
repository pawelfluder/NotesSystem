using TextHeaderAnalyzerCoreProj.Service;
using System.Text;
using SharpFileServiceProg.Operations.Headers;
using ElementType = TextHeaderAnalyzerCoreProj.ElementType;
using SharpRepoServiceProg.Service;
using SharpGoogleDocsProg.AAPublic;
using SharpNotesExporterProg.Register;
using SharpFileServiceProg.Service;

namespace SharpNotesExporter
{
    public class NotesExporterService
    {
        private readonly IGoogleDocsService docsService;
        private readonly IFileService fileService;
        private readonly HeaderNotesService headerNotesService;
        private readonly IRepoService repoService;
        private readonly HeadersOperations headersOp;

        public NotesExporterService(IRepoService repoService)
        {
            docsService = MyBorder.Container.Resolve<IGoogleDocsService>();
            fileService = MyBorder.Container.Resolve<IFileService>();
            headerNotesService = new HeaderNotesService();
            this.repoService = repoService;
            headersOp = fileService.Header;
        }

        public void GetMatch(string path, string docId)
        {
            var gg = headerNotesService.AnalyzeFile(path);
        }
        
        public void ExportNotesToGoogleDoc(
            string repo,
            string loca,
            string docId,
            bool createTwoColumns = false)
        {
            var textLines = repoService.Methods.GetTextLines((repo, loca));
            var name = repoService.Methods.GetLocalName((repo, loca));
            var elementsList = headerNotesService.GetElements2(textLines.Skip(4).ToArray());

            //var text = GetGoogleDocText(elementsList);

            // computations 1
            var tableSize = GetTableSize(elementsList);

            // stack 1
            docsService.StackWkr.LoadDocument(docId);
            if (docsService.StackWkr.LastIndex != 1)
            {
                docsService.StackWkr.StackDeleteAllContentRequest();
            }
            var s9 = docsService.StackWkr.ExecuteStack();

            docsService.StackWkr.StackInsertTextRequest(1, name);
            var s1 = docsService.StackWkr.ExecuteStack();
            docsService.StackWkr.StackInsertTableRequest(tableSize);
            var s2 = docsService.StackWkr.ExecuteStack();
            docsService.StackWkr.StackUpdateMarginsRequest((42, 14), (14, 14));
            var success1 = docsService.StackWkr.ExecuteStack();

            // computations 2
            var columnNumbers = Enumerable.Range(0, tableSize.ColumnCount - 1).ToList();
            var convertedList = headersOp.Convert.ToLinesList(elementsList);
            var cellsIndexes = docsService.StackWkr.GetFirstTableCellsIndexes();
            var neededIndexes = headersOp.Select.GetNeededIndexes(cellsIndexes, convertedList);
            headersOp.Select.CheckCorrectnes(neededIndexes, convertedList);
            var finalIndexes = headersOp.Select.FinalIndexes(neededIndexes, convertedList);

            // stack 2
            //var finalIndexes = GetFinalIndexes3(tableIndexes, elementsList);
            //var headersIndexes = GetOnlyHeaderIndexes(tableIndexes, elementsList);
            var tableMainIndex = docsService.StackWkr.GetFirstTableIndex();

            //stack
            docsService.StackWkr.StackUpdateTableColumnPropertiesRequest(tableMainIndex, 5, columnNumbers);
            var success2 = docsService.StackWkr.ExecuteStack();
            StackMergeCellsRequests(tableMainIndex, elementsList);
            var success3 = docsService.StackWkr.ExecuteStack();
            StackInsertTextRequests2(finalIndexes.ToList(), convertedList);
            var success4 = docsService.StackWkr.ExecuteStack();

            if (createTwoColumns)
            {
                docsService.StackWkr.StackTwoColumnsDocumentRequest();
                var success5 = docsService.StackWkr.ExecuteStack();
            }
        }

        public void StackInsertTextRequests2(
            List<(string Type, int Index)> finalIndexes,
            List<(string Type, int Level, object Value)> convertedList)
        {
            var i = 0;

            foreach (var index in finalIndexes)
            {
                var converted = convertedList[i];
                if (index.Item1 == ElementType.Header.ToString())
                {
                    docsService.StackWkr.StackInsertBoldTextRequests(index.Index, converted.Value.ToString());
                }

                if (index.Item1 == ElementType.LinesList.ToString())
                {
                    var tmp = converted.Value as List<string>;
                    var text = string.Join('\n', tmp);
                    docsService.StackWkr.StackInsertBoldTextRequests(index.Index, text);
                }

                i++;
            }
        }

        private List<string> ConvertToTextpartsList(
        List<(string Type, int Level, string Text)> elementsList)
        {
            var textpartsList = new List<string>();
            var temp = new StringBuilder();
            var newLine = "\n";
            var i = 0;
            var previousElem = elementsList.First();
            previousElem = default;

            for (; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];

                if (elem.Type == ElementType.Header.ToString())
                {
                    //if (temp != null) { textpartsList.Add(temp); temp = string.Empty; }
                    
                    if (temp.Length != 0)
                    {
                        temp.Remove(temp.Length - 1, 1);
                        textpartsList.Add(temp.ToString());
                        temp.Clear();
                    }

                    textpartsList.Add(elem.Text);
                }

                if (elem.Type == ElementType.Line.ToString())
                {
                    temp.Append(elem.Text + newLine);
                }

                previousElem = elem;
            }

            if (temp.Length != 0)
            {
                var elem = elementsList[i - 1];
                textpartsList.Add(temp.ToString());
            }

            return textpartsList;
        }

        public List<int> GetOnlyHeaderIndexes(
            List<int> tableIndexes,
            List<(string Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;
            var finalIndexes = new List<int>();

            var n = 0;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];

                if (elem.Type == ElementType.Header.ToString())
                {
                    var z = n + elem.Level - 2;
                    var index = tableIndexes[z];
                    finalIndexes.Add(index);
                    n = n + columnCount;
                }
            }

            return finalIndexes;
        }

        public List<int> GetFinalIndexes(
        List<int> tableIndexes,
        List<(string Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;
            var finalIndexes = new List<int>();

            var n = 0;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];

                if (elem.Type == ElementType.Header.ToString())
                {
                    var z = n + elem.Level - 2;
                    var index = tableIndexes[z];
                    finalIndexes.Add(index);
                    n = n + columnCount;
                }

                if (elem.Type == ElementType.Line.ToString() &&
                    previousElem.Type == ElementType.Header.ToString())
                {
                    var z = n + elem.Level - 1;
                    var index = tableIndexes[z];
                    finalIndexes.Add(index);
                    n = n + columnCount;
                }

                previousElem = elem;
            }

            return finalIndexes;
        }

        public List<(ElementType, int)> GetFinalIndexes2(
        List<int> tableIndexes,
        List<(ElementType Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;
            var finalIndexes = new List<(ElementType, int)>();

            var n = 0;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];

                if (elem.Type == ElementType.Header)
                {
                    var z = n + elem.Level - 2;
                    var index = tableIndexes[z];
                    finalIndexes.Add((ElementType.Header, index));
                    n = n + columnCount;
                }

                if (elem.Type == ElementType.Line && previousElem.Type == ElementType.Header)
                {
                    var z = n + elem.Level - 1;
                    var index = tableIndexes[z];
                    finalIndexes.Add((ElementType.LinesList, index));
                    n = n + columnCount;
                }

                previousElem = elem;
            }

            return finalIndexes;
        }

        public List<(string, int)> GetFinalIndexes3(
        List<int> cellsIndexes,
        List<(string Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;

            var finalIndexes = new List<(string, int)>();
            //var choosenIndexes = new List<(string, int)>();
            var convertedList = headersOp.Convert.ToLinesList(elementsList);
            var j = -1;
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                var rem = i % columnCount;
                int div = i / columnCount;
                if (rem == 0) { j++ ; }
                
                var elem = convertedList[j];
                var isTaken = IsTaken(elem, rem);
                if (isTaken)
                {
                    finalIndexes.Add((elem.Type, cellsIndexes[i]));
                }
            }
            var choosenIndexes = new List<(string, int)>(finalIndexes);
            var cellsNeededCount = CountCellsNeeded(elementsList);
            if (finalIndexes.Count != cellsNeededCount)
            {
                throw new Exception();
            }

            var r = 0;
            var addedCount = 0;
            for (int i = 0; i < convertedList.Count; i++)
            {
                var converted = convertedList[i];
                var index = finalIndexes[i];
                if (converted.Value is string converted2)
                {
                    finalIndexes[i] = (index.Item1, index.Item2 + addedCount);
                    //index = (index.Item1, index.Item2 + 1);
                    addedCount += converted2.Length;
                }
                if (converted.Value is List<string> tmp2)
                {
                    finalIndexes[i] = (index.Item1, index.Item2 + addedCount);
                    //index = (index.Item1, index.Item2 + tmp2.Count);
                    addedCount += string.Join('n', tmp2).Length;
                }
            }

            return finalIndexes;
        }

        public List<(string, int)> GetFinalIndexes4(
        List<int> cellsIndexes,
        List<(string Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;

            var finalIndexes = new List<(string, int)>();
            var convertedList = headersOp.Convert.ToLinesList(elementsList);
            var j = -1;
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                var rem = i % columnCount;
                int div = i / columnCount;
                if (rem == 0) { j++; }

                var elem = convertedList[j];
                var isTaken = IsTaken(elem, rem);
                if (isTaken)
                {
                    finalIndexes.Add((elem.Type, cellsIndexes[i]));
                }
            }


            var cellsNeededCount = CountCellsNeeded(elementsList);
            if (finalIndexes.Count != cellsNeededCount)
            {
                throw new Exception();
            }

            var r = 0;
            var count = 0;
            for (int i = 0; i < convertedList.Count; i++)
            {
                var converted = convertedList[i];
                var index = finalIndexes[i];
                if (converted.Value is string converted2)
                {
                    count += 1;
                    index = (index.Item1, index.Item2 + count);
                }
                if (converted.Value is List<string> tmp2)
                {
                    var lenght = string.Join('\n', tmp2).Length;
                    count += tmp2.Count;
                    index = (index.Item1, index.Item2 + 2 * count);
                }
            }

            return finalIndexes;
        }

        private int CountCellsNeeded(List<(string Type, int Level, string Text)> elemList)
        {
            var previousElem = elemList.First();
            previousElem = default;

            var tmp = new List<(string Type, int Level, string Text)>();
            foreach (var elem in elemList)
            {
                if (elem.Type == ElementType.Header.ToString())
                {
                    tmp.Add(elem);
                }

                if (elem.Type == ElementType.Line.ToString() &&
                    previousElem.Type == ElementType.Header.ToString())
                {
                    tmp.Add(elem);
                }

                previousElem = elem;
            }

            return tmp.Count;
        }

        private bool IsTaken(
            (string Type, int Level, object value) elem,
            int n)
        {
            int n2 = -1;
            if (elem.Type == ElementType.LinesList.ToString())
            {
                n2 = elem.Level;
            }

            if (elem.Type == ElementType.Header.ToString())
            {
                n2 = elem.Level - 2;
            }

            if (n == n2)
            {
                return true;
            }

            return false;
        }

        public void StackMergeCellsRequests(
            int tableIndex,
            List<(string Type, int Level, string Text)> elementsList)
        {
            var columnCount = elementsList.Select(x => x.Level).Max();
            var previousElem = elementsList.First();
            previousElem = default;
            var rowIndex = 0;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];

                if (elem.Type == ElementType.Header.ToString())
                {
                    rowIndex++;
                    var colIndex = elem.Level - 1;
                    var rowSpan = 1;
                    var colSpan = columnCount - elem.Level + 2;
                    docsService.StackWkr.StackMergeCellsRequest(
                        (rowIndex, colIndex),
                        tableIndex,
                        (rowSpan, colSpan));
                }

                if (elem.Type == ElementType.Line.ToString() &&
                    previousElem.Type == ElementType.Header.ToString())
                {
                    rowIndex++;
                    var colIndex = elem.Level;
                    var rowSpan = 1;
                    var colSpan = columnCount - elem.Level + 1;
                    docsService.StackWkr.StackMergeCellsRequest(
                        (rowIndex, colIndex),
                        tableIndex,
                        (rowSpan, colSpan));
                }

                previousElem = elem;
            }
        }

        private void StackMergeCellsRequest(
            int rowIndex,
            int columnCount,
            int tableIndex,
            (ElementType Type, int Level, string Text) elem)
        {
            var colIndex = elem.Level;
            var rowSpan = 1;
            var colSpan = columnCount - elem.Level;
            docsService.StackWkr.StackMergeCellsRequest(
                (rowIndex, colIndex),
                tableIndex,
                (rowSpan, colSpan));
        }


        public (int RowCount, int ColumnCount) GetTableSize(List<(string Type, int Level, string Text)> elementsList)//, string repoName, string repo)
        {
            if (elementsList.Count() == 0)
            {
                return (0, 0);
            }

            var rowCount = 0;
            var columnCount = elementsList.Select(x => x.Level).Max();

            var previousElem = elementsList.First();
            previousElem = default;

            for (int i = 0; i < elementsList.Count; i++)
            {
                var elem = elementsList[i];
                //var elem = elementsList[i];
                if (elem.Type == ElementType.Header.ToString())
                {
                    rowCount++;
                }

                if (elem.Type == ElementType.Line.ToString())
                {
                    if (previousElem.Type == ElementType.Header.ToString())
                    {
                        rowCount++;
                    }
                }

                previousElem = elem;
            }

            return (rowCount, columnCount);
        }

        public List<(int, int)> GetHeaderIndexes(List<(ElementType Type, int Level, string Text)> elementsList)//, string repoName, string repo)
        {
            var indexesList = new List<(int, int)>();
            var currentIndex = 1;

            foreach (var elem in elementsList)
            {
                if (elem.Type == ElementType.Line)
                {
                    currentIndex += elem.Text.Length + 2 * (elem.Level - 1);
                }

                if (elem.Type == ElementType.Header)
                {
                    indexesList.Add((currentIndex, currentIndex + elem.Text.Length + 1));
                    currentIndex += elem.Text.Length + 2 * (elem.Level - 2);
                }
            }

            return indexesList;
        }

        public string GetGoogleDocText(List<(string Type, int Level, string Text)> elementsList)//, string repoName, string repo)
        {
            var text = new StringBuilder();
            var newLine = '\n';
            //var newLine = "";

            foreach (var elem in elementsList)
            {
                if (elem.Type == ElementType.Line.ToString())
                {
                    var indentation = string.Concat(Enumerable.Repeat("  ", elem.Level - 1));
                    text.Append(indentation);
                    text.Append(elem.Text);
                    text.Append(newLine);
                }

                if (elem.Type == ElementType.Header.ToString())
                {
                    var indentation = string.Concat(Enumerable.Repeat("  ", elem.Level - 2));
                    text.Append(indentation);
                    text.Append(elem.Text);
                    text.Append(newLine);
                }
            }

            return text.ToString();
        }
    }
}
