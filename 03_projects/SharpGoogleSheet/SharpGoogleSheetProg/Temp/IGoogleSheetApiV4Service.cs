using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace GoogleApiV4CoreApp
{
   public interface IGoogleSheetApiV4Service
   {
      List<Request> PasteDataToSheet(
         int sheetId,
         string spreadsheetId,
         List<RowData> dataRowList,
         int columnStart,
         List<string> columnsList);

      void PasteDataAndFunctionsToSheet(
         string spreadsheetId,
         string sheetId,
         IList<IList<object>> data,
         List<string> propertyNames,
         List<(string, string)> formulas);

      IList<IList<object>> GetSheetData(string spreadsheetId, string sheetId, string range);

      List<Dictionary<object, object>> ConvertToListOfDictionaries(IList<IList<object>> input, List<string> keys);

      Spreadsheet GetSpreadsheet(string spreadsheetId);
      int GetSheetIdByTabName(string spreadsheetId, string copySheetTabName);
   }
}
