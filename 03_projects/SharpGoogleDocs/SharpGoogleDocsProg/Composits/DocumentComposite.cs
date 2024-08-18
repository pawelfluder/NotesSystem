using System.Text.RegularExpressions;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using GoogleDocument = Google.Apis.Docs.v1.Data.Document;

namespace SharpGoogleDocsProg.Composits;

public class DocumentComposite
{
    private DocsService _service;
    private GoogleDocument _document;
    private string _docId;
    private int _lastIndex;
    public int LastIndex => _lastIndex;
    public GoogleDocument Document => _document;

    public string DocId => _docId;

    public DocumentComposite(DocsService service)
    {
        _service = service;
    }

    public void ReLoadDocument()
    {
        LoadDocument(_docId);
    }

    public void LoadDocument(string docId)
    {
        var request = _service.Documents.Get(docId);
        _document = request.Execute();
        _docId = _document.DocumentId;
        _lastIndex = (int)_document.Body.Content.Last().EndIndex - 1;
    }
    
    public string GetIdFromPhotoUri(string uri)
    {
        string pattern = @"https:\/\/drive.google.com\/u\/0\/uc\?id=([^&]+)&export=download";
        Match match = Regex.Match(uri, pattern);
        if (match.Success)
        {
            string id = match.Groups[1].Value;
            return id;
        }

        throw new InvalidOperationException();
    }
    
    public int GetDocumentLastIndex(GoogleDocument document)
    {
        var lastEndIndex = (int)document.Body.Content.Last().EndIndex - 1;
        return lastEndIndex;
    }
    
    public int GetDocumentLastIndex(string docId)
    {
        var request = _service.Documents.Get(docId);
        var document = request.Execute();
        _document = document;
        _docId = document.DocumentId;
        _lastIndex = (int)document.Body.Content.Last().EndIndex - 1;
        return _lastIndex;
    }
    
    // GetTableFirstCellLastIndex
    public int GetLastIndexOfFirstCellOfFistTable()
    {
        var firstTable = GetFirstTable();
        var firstCell = GetFirstCell(firstTable);
        int? lastIndex = firstCell.EndIndex - 1;
        return lastIndex ?? -1;
    }

    private TableCell GetFirstCell(Table firstTable)
    {
        var firstRow = firstTable.TableRows.FirstOrDefault();
        TableCell? firstCell = firstRow.TableCells.FirstOrDefault();
        return firstCell;
    }

    public List<int> GetFirstTableIndexes(GoogleDocument document)
    {
        var table = document.Body.Content.FirstOrDefault(x => x.Table != null).Table;
        var indexes = GetTableIndexes(table);
        return indexes;
    }
    
    public List<int> GetTableIndexes(Table table)
    {
        var indexes = new List<int>();
        foreach (var row in table.TableRows)
        {
            foreach (var cell in row.TableCells)
            {
                //var index = cell.Content[0].StartIndex;
                var index = cell.StartIndex + 1;
                //var index = cell.StartIndex;
                indexes.Add(index ?? default);
            }
        }
        return indexes;
    }

    public Table GetFirstTable()
    {
        Table? table = _document.Body.Content.FirstOrDefault(x => x.Table != null).Table;
        return table;
    }
    
    public int GetFirstTableIndex()
    {
        var firstTableElement = _document.Body.Content.FirstOrDefault(x => x.Table != null);
        if (firstTableElement == null)
        {
            throw new Exception();
        }
    
        var index = firstTableElement.StartIndex;
        return (int)index;
    }
}