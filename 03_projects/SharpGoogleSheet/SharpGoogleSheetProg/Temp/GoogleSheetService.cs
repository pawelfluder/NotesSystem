using Google.Apis.Sheets.v4.Data;

namespace GoogleApiV4CoreApp
{
    public class GoogleSheetService
    {
        private readonly GoogleSheetService_Logic logicWorker;
        private readonly GoogleSheetService_Query queryWoker;

        public GoogleSheetService(string clientId, string clientSecret)
        {
            queryWoker = new GoogleSheetService_Query();
            queryWoker.Initialize(clientId, clientSecret);
            logicWorker = new GoogleSheetService_Logic(queryWoker);
        }

        public GoogleSheetService()
        {
        }

        public void PasteDataAndFunctionsToSheet(
           string spreadsheetId,
           string sheetId,
           IList<IList<object>> data,
           List<string> propertyNames,
           List<(string, string)> formulas)
        {
            var afterPropertiesColumnNumber = propertyNames.Count() + 1;
            var formulaNames = formulas.Select(x => x.Item1).ToList();
            var sId = int.Parse(sheetId);

            var dataRowList = data.Select(x => logicWorker.CreateDataRow(x)).ToList();
            var request1 = PasteDataToSheet(sId, spreadsheetId, dataRowList, 1, propertyNames);
            queryWoker.GetAllUpdateRequests(request1, spreadsheetId);

            if (formulas.Any())
            {
                var formulaList = GetFormulaDataList(data.Count(), formulas);
                var formulaRowList = formulaList.Select(x => logicWorker.CreateFormulaRow(x)).ToList();
                var request2 = PasteDataToSheet(sId, spreadsheetId, formulaRowList, afterPropertiesColumnNumber, formulaNames);
                queryWoker.GetAllUpdateRequests(request2, spreadsheetId);

            }
        }

        public List<Request> PasteDataToSheet(
           int sheetId,
           string spreadsheetId,
           List<RowData> dataRowList,
           int columnStartNumber,
           List<string> columnsList)
        {
            var dataValuesCount = dataRowList.First().Values.Count();
            var headersPosition = (1, columnStartNumber);
            var sampleRowCoordinates = (2, columnStartNumber);
            var dataStartCoordinates = (4, columnStartNumber);
            var columnEndNumber = columnStartNumber + dataValuesCount - 1;

            var requests = new List<Request>();
            requests.Add(logicWorker.AddOrDeleteColumn(spreadsheetId, sheetId, columnStartNumber, columnEndNumber));
            requests.Add(logicWorker.ClearAllCells(sheetId, columnStartNumber, columnEndNumber));
            requests.Add(logicWorker.CreateHeadersUpdate(sheetId, headersPosition, columnsList));
            requests.Add(logicWorker.CreateSampleRow(sheetId, sampleRowCoordinates, dataValuesCount));
            requests.Add(logicWorker.CreateUpdateRow(spreadsheetId, sheetId, dataStartCoordinates, dataRowList));
            requests.Add(logicWorker.AutoResize(sheetId, 1, dataValuesCount));

            return requests;
        }

        public IList<IList<object>> GetFormulaDataList(
           int dataCount,
           List<(string, string)> formulas)
        {
            var data = new List<IList<object>>();
            var offset = 4;
            for (int i = offset; i < dataCount + offset; i++)
            {
                var tmp = formulas.Select(x => (object)string.Format(x.Item2, i)).ToList();
                data.Add(tmp);
            }

            return data;
        }

        public IList<IList<object>> GetSheetData(string spreadsheetId, string sheetId, string range)
        {
            return logicWorker.GetSheetData(spreadsheetId, sheetId, range);
        }

        public List<Dictionary<object, object>> ConvertToListOfDictionaries(IList<IList<object>> input, List<string> keys)
        {
            return logicWorker.ConvertToListOfDictionaries(input, keys);
        }

        public Spreadsheet GetSpreadsheet(string spreadsheetId)
        {
            var dataFilters = new List<DataFilter>();
            var requestBody = new GetSpreadsheetByDataFilterRequest()
            {
                DataFilters = dataFilters,
                IncludeGridData = false,
            };

            var sheetsFile = logicWorker.ExecuteDataFilterRequest(requestBody, spreadsheetId);
            return sheetsFile;
        }

        public int GetSheetIdByTabName(string spreadsheetId, string copySheetTabName)
        {
            var sheetId = queryWoker.GetSheetIdByTabName(spreadsheetId, copySheetTabName);
            return sheetId;
        }
    }
}