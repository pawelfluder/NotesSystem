using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace SharpGoogleSheetProg.Service
{
    public class SheetWorker
    {
        private string sep = ";";
        private readonly GoogleSheetService parentService;
        private SheetsService sheetsService;
        private (string Id, string Name) tempFolder;
        private List<Request> stack;

        public SheetWorker(
            GoogleSheetService parentService,
            SheetsService service)
        {
            this.parentService = parentService;
            this.sheetsService = service;
            stack = new List<Request>();
        }

        public bool ExecuteStack(string spreadsheetId)
        {
            var stack2 = stack.Where(x => x != null).ToList();
            var success = TryExecuteBatchUpdate(stack2, spreadsheetId);
            stack.Clear();
            return success;
        }

        private bool TryExecuteBatchUpdate(
            IEnumerable<Request> requests,
            string spreadsheetId)
        {
            try
            {
                var batch = new BatchUpdateSpreadsheetRequest { Requests = requests.ToList() };
                var res = sheetsService.Spreadsheets.BatchUpdate(batch, spreadsheetId).Execute();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Request CreateSampleRow(int sheetId, (int, int) headersPosition, int dataMax)
        {
            var start = GetCoordinate(headersPosition, sheetId);

            var updateFormulasRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = start,
                    Fields = "*",
                }
            };

            var cellList = Enumerable.Repeat("x", dataMax).Select(x => CreateDataCell(x, false)).ToList();

            //cellList.AddRange(new List<string> { GetFormulaOfName(2), GetFormulaOfWhoOne(2), GetFormulaOfWhoTwo(2) }.Select(x =>
            //CreateFormulaCell(x, false)));

            var row = new RowData { Values = cellList };
            updateFormulasRequest.UpdateCells.Rows = new List<RowData> { row };

            return updateFormulasRequest;
        }

        public Spreadsheet GetSpreadsheet(string spreadsheetId)
        {
            var dataFilters = new List<DataFilter>();
            var requestBody = new GetSpreadsheetByDataFilterRequest()
            {
                DataFilters = dataFilters,
                IncludeGridData = false,
            };

            var dataFilterRequest = sheetsService.Spreadsheets.GetByDataFilter(requestBody, spreadsheetId);
            var sheetsFile = dataFilterRequest.Execute();
            return sheetsFile;
        }

        private string GetNextId(IList<IList<object>> data)
        {
            var stringIds = data.Select(x => x.ElementAt(0).ToString());
            var ids = stringIds.Select(x => int.Parse(x));
            var last = ids.Max();
            var next = last + 1;
            return next.ToString();
        }

        //public void PasteDataAndFunctionsToSheet(
        //   string spreadsheetId,
        //   string sheetId,
        //   IList<IList<object>> data,
        //   List<string> propertyNames,
        //   List<(string, string)> formulas)
        //{
        //    var afterPropertiesColumnNumber = propertyNames.Count() + 1;
        //    var formulaNames = formulas.Select(x => x.Item1).ToList();
        //    var sId = int.Parse(sheetId);

        //    var dataRowList = data.Select(x => logicWorker.CreateDataRow(x)).ToList();
        //    var request1 = PasteDataToSheet(sId, spreadsheetId, dataRowList, 1, propertyNames);
        //    queryWoker.GetAllUpdateRequests(request1, spreadsheetId);

        //    if (formulas.Any())
        //    {
        //        var formulaList = GetFormulaDataList(data.Count(), formulas);
        //        var formulaRowList = formulaList.Select(x => logicWorker.CreateFormulaRow(x)).ToList();
        //        var request2 = PasteDataToSheet(sId, spreadsheetId, formulaRowList, afterPropertiesColumnNumber, formulaNames);
        //        queryWoker.GetAllUpdateRequests(request2, spreadsheetId);

        //    }

        //}

        public void PasteDataToSheet(
            string spreadsheetId,
            string sheetName,
            IList<IList<object>> data,
            List<string> columnsList,
            Dictionary<char, string>? formulas = null)
        {
            if (data.Count() == 0)
            {
                return;
            }

            int dataHeight = data.Count();
            int dataWidth = data.First().Count;

            var headersPosition = (1, 1);
            var sampleRowCoordinates = (2, 1);
            var dataStartCoordinates = (4, 1);

            var sheetId = GetSheetId(spreadsheetId, sheetName);
            var columnCount = GetSheetColumnCount(spreadsheetId, sheetId) ?? default;

            stack.Add(ClearAllCells(sheetId, 1, dataWidth));
            stack.Add(AddOrDeleteColumn(sheetId, columnCount, dataWidth));

            stack.Add(CreateHeadersUpdate(sheetId, headersPosition, columnsList));
            stack.Add(CreateSampleRow(sheetId, sampleRowCoordinates, dataWidth));

            stack.Add(CreateUpdateRow(spreadsheetId, sheetId, dataStartCoordinates, data));

            stack.Add(CreateUpdateFormulas(sheetId, dataHeight, formulas));
            stack.Add(CopyPasteFormulas(sheetId, dataHeight, formulas));

            stack.Add(AutoResize(sheetId, 1, dataWidth));

            var success = ExecuteStack(spreadsheetId);
        }


        public void SynchMp3FilesWithSheet(IList<object> mp3NameList, IList<object> gg)
        {
            var idStatystykiNagrania = "1e7QIR6b7Kn8Hq5qsZvLeomd-me8xZmsTGubhbsKCIDA";

            //var gg2 = GetSheetData(idStatystykiNagrania, "A3:A");

            //var sheetId = GetSheetId(sheetsService, spreadsheetId, sheetName);

            //var requests = new BatchUpdateSpreadsheetRequest { Requests = new List<Request>() };
            //requests.Requests.Add(ClearAllCellsRequest(sheetId, 1, max));
            //requests.Requests.Add(AddOrDeleteColumnRequest(sheetId, columnCount, max));

            //var gg31 = 

            var gg11 = mp3NameList.Select(x => Path.GetFileNameWithoutExtension(x.ToString()));

            var gg7 = gg11.ElementAt(11);

            var gg9 = gg.Select(x => x.ToString().Contains(gg7.ToString()));

            var gg4 = gg11.Select(x => (x, gg.FirstOrDefault(y => y.ToString().Contains(x.ToString()))));

            var gg6 = gg4.Where(x => x.Item2 != null);

            var ListOfList = new List<IList<object>>();


            var gg1 = mp3NameList.Count();
            var gg2 = gg.Count();
            var gg3 = Math.Max(gg1, gg2);

            object mp3FileName;
            object sheetAproachName;

            for (int i = 0; i < gg3; i++)
            {
                var next = new List<object>();

                if (mp3NameList.Count - 1 >= i)
                {
                    mp3FileName = Path.GetFileNameWithoutExtension(mp3NameList[i].ToString());
                    next.Add(mp3FileName);
                }
                else
                {
                    next.Add(string.Empty);
                }

                if (gg.Count - 1 >= i)
                {
                    sheetAproachName = gg[i];
                    next.Add(sheetAproachName);
                }
                else
                {
                    next.Add(string.Empty);
                }

                ListOfList.Add(next);
            }
            var columnsList = new List<string>();
            PasteDataToSheet(idStatystykiNagrania, "Nagrania_Dane", ListOfList, columnsList);

        }

        private int? GetSheetColumnCount(string spreadsheetId, int sheetId)
        {
            var dataFilters = new List<DataFilter>();
            var requestBody = new GetSpreadsheetByDataFilterRequest()
            {
                DataFilters = dataFilters,
                IncludeGridData = false,
            };

            var dataFilterRequest = sheetsService.Spreadsheets.GetByDataFilter(requestBody, spreadsheetId);
            var sheetsFile = dataFilterRequest.Execute();
            var sheet = sheetsFile.Sheets.Single(x => x.Properties.SheetId == sheetId);
            return sheet.Properties.GridProperties.ColumnCount;
        }

        private Request CreateHeadersUpdate(int sheetId, (int, int) sc, List<string> columnsList)
        {
            var start = GetCoordinate(sc, sheetId);

            var updateFormulasRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = start,
                    Fields = "*",
                }
            };

            var formulasColumnsList = new List<string>() { "FileName", "Who1", "Who2" };

            var cellList = columnsList
               .Select(x => CreateDataCell(x, false)).ToList();

            var row = new RowData { Values = cellList };
            updateFormulasRequest.UpdateCells.Rows = new List<RowData> { row };

            return updateFormulasRequest;
        }

        private Request AddOrDeleteColumn(int sheetId, int columnNumber, int max)
        {
            Request request = null;

            if (columnNumber < max)
            {
                request = new Request
                {
                    InsertDimension = new InsertDimensionRequest()
                    {
                        Range = new DimensionRange()
                        {
                            SheetId = sheetId,
                            Dimension = "COLUMNS",
                            StartIndex = columnNumber - 1,
                            EndIndex = max - 1,
                        }
                    }
                };
            }
            else if (columnNumber > max)
            {
                request = new Request
                {
                    DeleteDimension = new DeleteDimensionRequest()
                    {
                        Range = new DimensionRange()
                        {
                            SheetId = sheetId,
                            Dimension = "COLUMNS",
                            StartIndex = max - 1,
                            EndIndex = columnNumber - 1
                        }
                    }
                };
            }

            return request;
        }

        public Dictionary<char, string> GetFormulas()
        {
            var f = new Dictionary<char, string>();
            f.Add('D', "=C4-B4");
            return f;
        }

        private Request CopyPasteFormulas(
            int sheetId,
            int dataHeight,
            Dictionary<char, string>? formulas)
        {
            if (formulas == null)
            {
                return null;
            }

            var f = formulas.First();
            (int, int) cor = LetterToSc(f);

            var request = new Request
            {
                CopyPaste = new CopyPasteRequest
                {
                    PasteType = "paste_normal",
                    Source = new GridRange()
                    {
                        StartRowIndex = cor.Item1,
                        EndRowIndex = cor.Item1+1,
                        SheetId = sheetId,
                        StartColumnIndex = cor.Item2,
                        EndColumnIndex = cor.Item2+1,
                        
                    },
                    Destination = new GridRange()
                    {
                        StartRowIndex = cor.Item2,
                        EndRowIndex = dataHeight+3,
                        SheetId = sheetId,
                        StartColumnIndex = cor.Item2,
                        EndColumnIndex = cor.Item2 + 1,
                    },
                }
            };

            return request;
        }

        private Request CreateUpdateFormulas(
            int sheetId,
            int dataHeight,
            Dictionary<char, string>? formulas)
        {
            if (formulas == null)
            {
                return null;
            }

            var f = formulas.First();

            (int, int) cor = LetterToSc(f);
            var gridCoordinates = GetCoordinate(cor, sheetId);

            var request = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = gridCoordinates,
                    Fields = "*",
                }
            };

            var r = new RowData();
            var listOfList = new List<RowData>() { r };
            r.Values = new List<CellData>();
            r.Values.Add(CreateFormulaCell(f.Value,false));
            request.UpdateCells.Rows = listOfList;

            return request;
        }

        private (int, int) LetterToSc(KeyValuePair<char, string> f)
        {
            int index = char.ToUpper(f.Key) - 65;
            return (3, index);
        }

        //private Request CreateUpdateFormulas(int sheetId, (int, int) sc, int dataCount)
        //{
        //    var start = GetCoordinate(sc, sheetId);

        //    var updateFormulasRequest = new Request
        //    {
        //        UpdateCells = new UpdateCellsRequest
        //        {
        //            Start = start,
        //            Fields = "*",
        //        }
        //    };

        //    var listOfList = new List<RowData>();

        //    for (int i = 0; i < dataCount; i++)
        //    {
        //        var formula1 = CreateFormulaCell(GetFormulaOfName(i + sc.Item1), false);
        //        var formula2 = CreateFormulaCell(GetFormulaOfWhoOne(i + sc.Item1), false);
        //        var formula3 = CreateFormulaCell(GetFormulaOfWhoTwo(i + sc.Item1), false);
        //        var cellList = new List<CellData> { formula1, formula2, formula3 };
        //        var row = new RowData { Values = cellList };
        //        listOfList.Add(row);
        //    }

        //    updateFormulasRequest.UpdateCells.Rows = listOfList;

        //    return updateFormulasRequest;
        //}

        private string GetFormulaOfWhoOne(int number)
        {
            var cn1 = "B"; //Column letter one
            var cn2 = "C"; //Column letter two

            var formula = string.Format(
               "={2}{0}&IF(AND({2}{0}<>\"\"{1}{3}{0}<>\"\"){1}\"_\"{1}\"\")&{3}{0}",
               number, sep, cn1, cn2);

            return formula;
        }

        private string GetFormulaOfWhoTwo(int number)
        {
            var cn1 = "D"; //Column letter one
            var cn2 = "E"; //Column letter two
            var cn3 = "F"; //Column letter three

            var formula = string.Format(
            "=IF({2}{0}=\"x\"{1}\"\"{1}{2}{0})&IF(OR(AND({2}{0}<>\"x\"{1}{3}{0}<>\"x\"){1}AND({2}{0}<>\"x\"{1}{4}{0}<>\"x\")){1}\"_\"{1}\"\")&IF({3}{0}=\"x\"{1}\"\"{1}{3}{0})&IF(AND({3}{0}<>\"x\"{1}{4}{0}<>\"x\"){1}\"_\"{1}\"\")&IF({4}{0}=\"x\"{1}\"\"{1}{4}{0})",
            number, sep, cn1, cn2, cn3);
            //"=IF(E{0}=\"x\"{1}\"\"{1}E{0})&IF(OR(AND(E{0}<>\"x\"{1}F{0}<>\"x\"){1}AND(E{0}<>\"x\"{1}G{0}<>\"x\")){1}\"_\"{1}\"\")&IF(F{0}=\"x\"{1}\"\"{1}F{0})&IF(AND(F{0}<>\"x\"{1}G{0}<>\"x\"){1}\"_\"{1}\"\")&IF(G{0}=\"x\"{1}\"\"{1}G{0})",
            return formula;
        }

        private string GetFormulaOfName(int number)
        {
            var cn1 = "H"; //Column letter one
            var cn2 = "I"; //Column letter two
            var formula = string.Format(
               "={2}{0}&IF(AND({2}{0}<>\"\"{1}{3}{0}<>\"\"){1}\"_\"{1}\"\")&{3}{0}",
               number, sep, cn1, cn2);
            //"=I{0}&IF(AND(I{0}<>\"\"{1}J{0}<>\"\"){1}\"_\"{1}\"\")&J{0}",

            return formula;
        }

        private Request CreateUpdateRow(string spreadsheetId, int sheetId, (int, int) startCoordinates, IList<IList<object>> dataListOfList)
        {
            var listRowData = new List<RowData>();

            foreach (var dataList in dataListOfList)
            {
                IfNullWriteProblemDescriptionToConsole(dataList);
                var values = dataList.Select(x => CreateDataCell(x, false)).ToList();
                listRowData.Add(new RowData { Values = values });
            }

            var updateRowRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = GetCoordinate(startCoordinates, sheetId),
                    Fields = "*",
                }
            };

            updateRowRequest.UpdateCells.Rows = listRowData;
            return updateRowRequest;
        }
        private void IfNullWriteProblemDescriptionToConsole(IList<object> list)
        {
            foreach (var item in list)
            {
                if (item == null)
                {
                    var id = list[0];
                    var description = $"In id ={id} value is null";
                }
            }
        }


        private Request AddNextId(string spreadsheetId, int sheetId, (int, int) startCoordinates, string nextId)
        {
            var values = new List<CellData> { CreateDataCell(nextId, false) };
            var nextIdRow = new RowData { Values = values };
            var listRowData = new List<RowData> { nextIdRow };

            var updateRowRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = GetCoordinate(startCoordinates, sheetId),
                    Fields = "*",
                }
            };

            updateRowRequest.UpdateCells.Rows = listRowData;
            return updateRowRequest;
        }

        private GridCoordinate GetCoordinate((int, int) startCoordinates, int sheetId)
        {
            return new GridCoordinate
            {
                ColumnIndex = startCoordinates.Item2 - 1,
                RowIndex = startCoordinates.Item1 - 1,
                SheetId = sheetId,
            };
        }

        private Request ClearAllCells(int sheetId, int startIndex, int endIndex)
        {
            //var insertRow = new Request
            //{
            //   DeleteDimension = new DeleteDimensionRequest()
            //   {
            //      Range = new DimensionRange()
            //      {
            //         SheetId = sheetId,
            //         Dimension = "COLUMNS",
            //         StartIndex = startIndex - 1,
            //         EndIndex = endIndex - 1
            //      }
            //   }
            //};

            var clearAllCellsRequest = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Range = new GridRange()
                    {
                        SheetId = sheetId,
                    },
                    Fields = "*",
                }
            };

            var insertRow = new Request
            {
                ClearBasicFilter = new ClearBasicFilterRequest
                {
                    SheetId = sheetId,
                }
            };

            return clearAllCellsRequest;
        }

        private Request AutoResize(int sheetId, int startIndex, int endIndex)
        {
            var request = new Request
            {
                AutoResizeDimensions = new AutoResizeDimensionsRequest()
                {
                    Dimensions = new DimensionRange()
                    {
                        SheetId = sheetId,
                        Dimension = "COLUMNS",
                        StartIndex = startIndex - 1,
                        EndIndex = endIndex - 1
                    }
                }
            };

            return request;
        }

        private Request CreateInsertEmptyRowRequest(int sheetId, int startIndex, int endIndex)
        {
            var insertRow = new Request
            {
                InsertDimension = new InsertDimensionRequest()
                {
                    Range = new DimensionRange()
                    {
                        SheetId = sheetId,
                        Dimension = "ROWS",
                        StartIndex = startIndex - 1,
                        EndIndex = endIndex - 1
                    }
                }
            };

            return insertRow;
        }

        private CellData CreateDataCell(object input, bool isBold) => CreateCell(input, isBold, false);

        private CellData CreateFormulaCell(object input, bool isBold) => CreateCell(input, isBold, true);

        private CellData CreateCell(object input, bool isBold, bool isFormula)
        {
            var cellData = new CellData();
            ExtendedValue extendedValue = new ExtendedValue();
            CellFormat cellFormat = new CellFormat { TextFormat = new TextFormat() };

            if (isFormula)
            {
                extendedValue.FormulaValue = input.ToString();
            }
            else
            {
                var success = int.TryParse(input?.ToString(), out var number);
                if (success == true)
                {
                    extendedValue.NumberValue = number;
                }
                else
                {
                    extendedValue.StringValue = input?.ToString();
                }
            }

            if (isBold)
            {
                cellFormat.TextFormat.Bold = true;

                //cellFormat.;
            }

            //cellFormat.BackgroundColor = new Color { Blue = (float)cell.BackgroundColor.B / 255, Red = (float)cell.BackgroundColor.R / 255, Green = (float)cell.BackgroundColor.G / 255 };

            cellData.UserEnteredFormat = cellFormat;
            cellData.UserEnteredValue = extendedValue;

            return cellData;
        }

        public void Method(string spreadsheetId)
        {

            // Add new Sheet
            string sheetName = string.Format("{0} {1}", DateTime.Now.Month, DateTime.Now.Day);
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = sheetName;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request
            {
                AddSheet = addSheetRequest
            });

            var batchUpdateRequest =
               sheetsService.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId);

            batchUpdateRequest.Execute();
        }

        private IList<IList<object>> GetSheetData(string spreadsheetId, string range)
        {
            var request =
               sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;

            return values;
        }

        public IList<IList<object>> GetSheetData(string spreadsheetId, string sheetId, string sheetRange)
        {
            var spreadsheet = sheetsService.Spreadsheets.Get(spreadsheetId).Execute();
            var sheet = spreadsheet.Sheets.SingleOrDefault(x => x.Properties.SheetId.ToString() == sheetId);
            var sheetName = sheet.Properties.Title;

            var generalRange = sheetName + "!" + sheetRange;
            var result = GetSheetData(spreadsheetId, generalRange);

            return result;
        }


        public List<Dictionary<object, object>> GetSheetData(string spreadsheetId, string range, List<string> keys)
        {
            var request =
               sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;

            return ConvertToListOfDictionaries(values, keys);
        }

        public List<Dictionary<object, object>> ConvertToListOfDictionaries(IList<IList<object>> input, List<string> keys)
        {
            var dictList = new List<Dictionary<object, object>>();

            if (input == null)
            {
                return dictList;
            }

            foreach (var list in input)
            {
                var dict = ListToKeyValue(keys, list);
                dictList.Add(dict);
            }

            return dictList;
        }

        private static Dictionary<object, object> ListToKeyValue(List<string> keys, IList<object> list)
        {
            var dict = new Dictionary<object, object>();
            var i = 0;
            foreach (var item in list)
            {
                dict.Add(keys.ElementAt(i), item);
                i++;
            }

            return dict;
        }

        public AppendValuesResponse AppendSheetData(ValueRange body, string spreadsheetId, string range)
        {
            var request =
               sheetsService.Spreadsheets.Values.Append(body, spreadsheetId, range);

            var response = request.Execute();
            return response;
        }

        public int GetSheetId(string spreadSheetId, string spreadSheetName)
        {
            var spreadsheet = sheetsService.Spreadsheets.Get(spreadSheetId).Execute();

            var sheet = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == spreadSheetName);
            int sheetId = (int)sheet.Properties.SheetId;
            return sheetId;
        }

        //private int GetSheetId(SheetsService service, string spreadSheetId, string spreadSheetName)
        //{
        //    var spreadsheet = service.Spreadsheets.Get(spreadSheetId).Execute();

        //    var sheet = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == spreadSheetName);
        //    int sheetId = (int)sheet.Properties.SheetId;
        //    return sheetId;
        //}
    }
}