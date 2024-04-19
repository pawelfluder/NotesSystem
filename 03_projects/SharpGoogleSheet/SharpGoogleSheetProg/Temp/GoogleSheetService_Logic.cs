using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoogleApiV4CoreApp
{
   internal class GoogleSheetService_Logic
   {
      GoogleSheetService_Query queryWoker;

      public GoogleSheetService_Logic(GoogleSheetService_Query queryWoker)
      {
         this.queryWoker = queryWoker;
      }

      public CellData CreateDataCell(object input, bool isBold) => CreateCell(input, isBold, false);

      public CellData CreateFormulaCell(object input, bool isBold) => CreateCell(input, isBold, true);

      internal RowData CreateDataRow(IList<object> inputList)
      {
         var cellList = inputList.Select(x => CreateDataCell(x, false)).ToList();
         var row = new RowData { Values = cellList };
         return row;
      }

      internal RowData CreateFormulaRow(IList<object> inputList)
      {
         var cellList = inputList.Select(x => CreateFormulaCell(x, false)).ToList();
         var row = new RowData { Values = cellList };
         return row;
      }

      internal int GetSheetId(string spreadSheetId, string spreadSheetName)
      {
         var spreadsheet = queryWoker.GetSpreadSheet(spreadSheetId);
         var sheet = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == spreadSheetName);
         int sheetId = (int)sheet.Properties.SheetId;
         return sheetId;
      }

      internal int? GetSheetColumnCount(string spreadsheetId, int sheetId)
      {
         var dataFilters = new List<DataFilter>();
         var requestBody = new GetSpreadsheetByDataFilterRequest()
         {
            DataFilters = dataFilters,
            IncludeGridData = false,
         };

         var dataFilterRequest = queryWoker.GetByDataFilter(requestBody, spreadsheetId);
         var sheetsFile = dataFilterRequest.Execute();
         var sheet = sheetsFile.Sheets.Single(x => x.Properties.SheetId == sheetId);
         return sheet.Properties.GridProperties.ColumnCount;
      }

      internal Request CreateHeadersUpdate(int sheetId, (int, int) sc, List<string> headerNamesList)
      {
         var start = GetHeadersCoordinates(sc, sheetId);

         var updateCellsRequest = new Request
         {
            UpdateCells = new UpdateCellsRequest
            {
               Start = start,
               Fields = "*",
            }
         };

         var cellList = headerNamesList.Select(x => CreateDataCell(x, false)).ToList();
         var row = new RowData { Values = cellList };
         updateCellsRequest.UpdateCells.Rows = new List<RowData> { row };

         return updateCellsRequest;
      }

      internal Request AddOrDeleteColumn(string spreadsheetId, int sheetId, int expectedColumnStartNumber, int expectedColumnEndNumber)
      {
         Request request = null;
         var expectedEndIndex = expectedColumnEndNumber - 1;
         var currentLastColumnNumber = GetSheetColumnCount(spreadsheetId, sheetId) ?? default;
         var currentLastColumnIndex = currentLastColumnNumber - 1;
         var diffrence = expectedEndIndex - currentLastColumnIndex;

         if (currentLastColumnIndex < expectedEndIndex)
         {
            request = new Request
            {
               AppendDimension = new AppendDimensionRequest()
               {
                  Length = diffrence,
                  Dimension = "COLUMNS",
                  SheetId = sheetId,
               }
            };
         }

         if (currentLastColumnIndex > expectedEndIndex)
         {
            request = new Request
            {
               DeleteDimension = new DeleteDimensionRequest()
               {

                  Range = new DimensionRange()
                  {
                     SheetId = sheetId,
                     Dimension = "COLUMNS",
                     StartIndex = expectedEndIndex,
                     EndIndex = currentLastColumnIndex,
                  }
               }
            };
         }

         return request;
      }

      public Spreadsheet ExecuteDataFilterRequest(GetSpreadsheetByDataFilterRequest requestBody, string spreadsheetId)
      {
         var dataFilterRequest = queryWoker.GetByDataFilter(requestBody, spreadsheetId);
         var sheetsFile = dataFilterRequest.Execute();
         return sheetsFile;
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

      private GridCoordinate GetHeadersCoordinates((int, int) startCoordinates, int sheetId)
      {
         return new GridCoordinate
         {
            RowIndex = startCoordinates.Item1 - 1,
            ColumnIndex = startCoordinates.Item2 - 1,
            SheetId = sheetId,
         };
      }

      public Request ClearAllCells(int sheetId, int startNumber, int endNumber)
      {
         var startIndex = startNumber - 1;
         var endIndex = endNumber - 1;

         var clearAllCellsRequest = new Request
         {
            UpdateCells = new UpdateCellsRequest
            {
               Range = new GridRange()
               {
                  SheetId = sheetId,
                  StartColumnIndex = startIndex,
                  EndColumnIndex = endNumber,
               },
               Fields = "*",
            }
         };

         return clearAllCellsRequest;
      }

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
            var success = int.TryParse(input.ToString(), out var number);
            if (success == true)
            {
               extendedValue.NumberValue = number;
            }
            else
            {
               extendedValue.StringValue = input.ToString();
            }
         }

         if (isBold)
         {
            cellFormat.TextFormat.Bold = true;
         }

         //cellFormat.BackgroundColor = new Color { Blue = (float)cell.BackgroundColor.B / 255, Red = (float)cell.BackgroundColor.R / 255, Green = (float)cell.BackgroundColor.G / 255 };

         cellData.UserEnteredFormat = cellFormat;
         cellData.UserEnteredValue = extendedValue;

         return cellData;
      }

      private IList<IList<object>> GetSheetData(string spreadsheetId, string range)
      {
         var values = queryWoker.GetSpreadSheetValues(spreadsheetId, range);
         return values;
      }

      public IList<IList<object>> GetSheetData(string spreadsheetId, string sheetId, string sheetRange)
      {
         var spreadsheet = queryWoker.GetSpreadSheet(spreadsheetId);
         var sheet = spreadsheet.Sheets.SingleOrDefault(x => x.Properties.SheetId.ToString() == sheetId);
         var sheetName = sheet.Properties.Title;

         var generalRange = sheetName + "!" + sheetRange;
         var result = GetSheetData(spreadsheetId, generalRange);

         return result;
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

      public AppendValuesResponse AppendSheetData(ValueRange body, string spreadsheetId, string range)
      {
         var response = queryWoker.AppendValues(body, spreadsheetId, range);
         return response;
      }

      private string GetNextId(IList<IList<object>> data)
      {
         var stringIds = data.Select(x => x.ElementAt(0).ToString());
         var ids = stringIds.Select(x => int.Parse(x));
         var last = ids.Max();
         var next = last + 1;
         return next.ToString();
         //return "";
      }

      internal Request CreateSampleRow(int sheetId, (int, int) headersPosition, int dataMax)
      {
         var start = GetHeadersCoordinates(headersPosition, sheetId);

         var updateFormulasRequest = new Request
         {
            UpdateCells = new UpdateCellsRequest
            {
               Start = start,
               Fields = "*",
            }
         };

         var cellList = Enumerable.Repeat("x", dataMax).Select(x => CreateDataCell(x, false)).ToList();



         var row = new RowData { Values = cellList };
         updateFormulasRequest.UpdateCells.Rows = new List<RowData> { row };

         return updateFormulasRequest;
      }

      //public List<Dictionary<object, object>> GetSheetData(string spreadsheetId, string range, List<string> keys)
      //{
      //   var values = queryWoker.GetSpreadSheetValues(spreadsheetId, range);
      //   return ConvertToListOfDictionaries(values, keys);
      //}

      internal Request AutoResize(int sheetId, int startIndex, int endIndex)
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

      //private Request CreateInsertEmptyRowRequest(int sheetId, int startIndex, int endIndex)
      //{
      //   var insertRow = new Request
      //   {
      //      InsertDimension = new InsertDimensionRequest()
      //      {
      //         Range = new DimensionRange()
      //         {
      //            SheetId = sheetId,
      //            Dimension = "ROWS",
      //            StartIndex = startIndex - 1,
      //            EndIndex = endIndex - 1
      //         }
      //      }
      //   };

      //   return insertRow;
      //}

      internal Request CreateUpdateRow(string spreadsheetId, int sheetId, (int, int) startCoordinates, List<RowData> dataRowList)
      {
         var updateRowRequest = new Request
         {
            UpdateCells = new UpdateCellsRequest
            {
               Start = GetHeadersCoordinates(startCoordinates, sheetId),
               Fields = "*",
            }
         };

         updateRowRequest.UpdateCells.Rows = dataRowList;
         return updateRowRequest;
      }

      private void SynchMp3FilesWithSheet(IList<object> mp3NameList, IList<object> gg)
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
         var gg10 = gg11.FirstOrDefault(x => x.ToString().Contains("Kamila"));

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
      }
   }
}
