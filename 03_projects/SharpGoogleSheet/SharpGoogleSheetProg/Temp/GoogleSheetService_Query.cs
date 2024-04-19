using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Google.Apis.Sheets.v4.SpreadsheetsResource;

namespace GoogleApiV4CoreApp
{
   internal class GoogleSheetService_Query
   {
      private string ApplicationName = "Google Sheets API .NET Quickstart";
      private SheetsService sheetsService;
      private string sep = ";";

      internal void Initialize(string clientId, string clientSecret)
      {
         var secrets = new ClientSecrets();
         secrets.ClientId = clientId;
         secrets.ClientSecret = clientSecret;
         string[] scopes = { SheetsService.ScopeConstants.Spreadsheets };

         var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
             secrets,
             scopes,
             "user",
             CancellationToken.None);

         sheetsService = new SheetsService(new BaseClientService.Initializer()
         {
            HttpClientInitializer = credentialAuthorization.Result,
            ApplicationName = ApplicationName,
         });
      }

      internal Spreadsheet GetSpreadSheet(string spreadSheetId)
      {
         var spreadSheet = sheetsService.Spreadsheets.Get(spreadSheetId).Execute();
         return spreadSheet;
      }

      internal GetByDataFilterRequest GetByDataFilter(GetSpreadsheetByDataFilterRequest requestBody, string spreadsheetId)
      {
         var dataFilterRequest = sheetsService.Spreadsheets.GetByDataFilter(requestBody, spreadsheetId);
         return dataFilterRequest;
      }

      internal void GetAllUpdateRequests(List<Request> requests, string spreadsheetId)
      {
         var batch = new BatchUpdateSpreadsheetRequest { Requests = requests };
         sheetsService.Spreadsheets.BatchUpdate(batch, spreadsheetId).Execute();
      }

      internal IList<IList<object>> GetSpreadSheetValues(string spreadsheetId, string range)
      {
         var response = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range).Execute();
         var values = response.Values;
         return values;
      }

      internal AppendValuesResponse AppendValues(ValueRange body, string spreadsheetId, string range)
      {
         var response = sheetsService.Spreadsheets.Values.Append(body, spreadsheetId, range).Execute();
         return response;
      }

      internal int GetSheetIdByTabName(string spreadSheetId, string copySheetTabName)
      {
         var spreadsheet = sheetsService.Spreadsheets.Get(spreadSheetId).Execute();
         var sheet = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == copySheetTabName);
         var sheetId = (int)sheet.Properties.SheetId;
         return sheetId;
      }
   }
}
