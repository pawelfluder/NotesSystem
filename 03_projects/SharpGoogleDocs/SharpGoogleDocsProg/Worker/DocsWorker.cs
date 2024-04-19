using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using GoogleDocument = Google.Apis.Docs.v1.Data.Document;
using GoogleDocsRange = Google.Apis.Docs.v1.Data.Range;

namespace SharpGoogleDocsProg.Worker
{
    public class DocsWorker
    {
        DocsService service;

        public DocsWorker(DocsService service)
        {
            this.service = service;
        }

        public string UpdateDocument(string id, List<string> lines, string sep)
        {
            var lastEndIndex = GetDocumentLastIndex(id);
            ClearDocument(id, lastEndIndex);
            AppendToDocument(id, lines, 1, sep);

            return null;
        }

        public int GetDocumentLastIndex(GoogleDocument document)
        {
            var lastEndIndex = (int)document.Body.Content.Last().EndIndex - 1;
            return lastEndIndex;
        }

        public int GetDocumentLastIndex(string id)
        {
            GoogleDocument document = GetDocument(id);
            var lastEndIndex = (int)document.Body.Content.Last().EndIndex - 1;
            return lastEndIndex;
        }

        public Request GetInsertPhotosRequests(int width, string uri, int index)
        {
            var gg = GetInsertPhotosRequest(width, uri, index);
            return gg;
        }

        public List<Request> GetUrlMessagesRequests(Dictionary<string, List<(string, string)>> input, GoogleDocument document)
        {
            var temp1 = document.Body.Content.Where(x => x.Paragraph != null).SelectMany(x => x.Paragraph.Elements);
            var requests = new List<Request>();

            foreach (var item in input)
            {
                if (item.Key == string.Empty)
                {
                    continue;
                }

                var temp = temp1.SingleOrDefault(x => x.TextRun.Content.StartsWith(item.Key));
                if (temp != null)
                {
                    var i = 0;
                    foreach (var value in item.Value)
                    {
                        i++;
                        var gg = temp1.SkipWhile(x => x != temp).Skip(i).FirstOrDefault();
                        if (gg.TextRun.Content.StartsWith(value.Item1))
                        {
                            var request = GetTextStyleUpdateRequest(((int)gg.StartIndex, (int)gg.EndIndex), value.Item2);
                            requests.Add(request);
                        }
                    }

                }
            }

            return requests;
        }

        public void ExecuteBatchUpdate(Request request, string id)
        {
            var requestsList = new List<Request>() { request };

            if (requestsList.Count > 0)
            {
                TryExecuteBatchUpdate(requestsList, id, 1);
            }
        }

        public void ExecuteBatchUpdate(List<Request> requestsList, string id)
        {
            if (requestsList.Count > 0)
            {
                TryExecuteBatchUpdate(requestsList, id, 1);
            }
        }

        public BatchUpdateDocumentResponse ExecuteBatchUpdateRequest(BatchUpdateDocumentRequest batchRequest, string id)
        {
            var result = service.Documents.BatchUpdate(batchRequest, id).Execute();
            return result;
        }

        public void TryExecuteBatchUpdate(List<Request> requestsList, string id)
        {
            TryExecuteBatchUpdate(requestsList, id, 1);
        }

        public void TryExecuteBatchUpdate(List<Request> requestsList, string id, int maxAttemptCount)
        {
            var attemptCount = 0;
            if (maxAttemptCount >= 1)
            {
                while (attemptCount != -1)
                {
                    try
                    {
                        var batchUpdateRequest = new BatchUpdateDocumentRequest()
                        {
                            Requests = requestsList,
                        };
                        var result = service.Documents.BatchUpdate(batchUpdateRequest, id).Execute();
                        return;
                    }
                    catch (Exception ex)
                    {
                        attemptCount++;
                    }

                    if (attemptCount > maxAttemptCount)
                    {
                        return;
                        //
                    }
                }
            }
        }



        private Request GetInsertTableRequest((int ColumnsCount, int RowsCount) size)
        {
            var request = new Request()
            {
                InsertTable = new InsertTableRequest()
                {
                    EndOfSegmentLocation = new EndOfSegmentLocation
                    {
                        SegmentId = ""
                    },
                    Columns = size.ColumnsCount,
                    Rows = size.RowsCount,
                }
            };

            return request;
        }

        public Request GetInsertTextRequest(int? lastEndIndex, string text)
        {
            var insertTextRequest = new Request()
            {
                InsertText = new InsertTextRequest()
                {
                    Location = new Location()
                    {
                        Index = lastEndIndex,
                    },
                    Text = text,
                },
            };

            return insertTextRequest;
        }

        public void AppendToDocument(string id, List<string> lines, int lastEndIndex, string sep)
        {
            var text = ConcatLines(lines, sep);
            AppendToDocument(id, text, lastEndIndex);
        }

        public void AppendToDocument(string id, string text, int lastEndIndex)
        {
            //https://developers.google.com/docs/api/how-tos/format-text

            var request = GetInsertTextRequest(lastEndIndex, text);
            ExecuteBatchUpdate(request, id);
        }

        public Request GetInsertPhotosRequest(int width, string uri, int index)
        {
            return new Request()
            {
                InsertInlineImage = new()
                {
                    //Uri = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQgI2LGva1JuYa--ODqOja1y0haWB7XPOXArUO2Lkdumw&s",
                    //Uri = "D:/01_Synchronized/01_Programming_Files/8c0f7763-7149-4b4d-9d6a-b28d3984552f/15_Zbior/PythonTinderApiDataExport/Output/ExportedApplicationData/635e47ac1e983b01004469ac/636971af18d4b20100b9d22c/640x800_ce036b17-abc8-4230-bb03-2be1690b74f8.jpg",
                    Uri = uri,
                    ObjectSize = new Size()
                    {
                        //Height = new Dimension()
                        //{
                        //    Magnitude = height,
                        //    Unit = "PT",
                        //},
                        Width = new Dimension()
                        {
                            Magnitude = width,
                            Unit = "PT",
                        }
                    },
                    Location = new Location()
                    {
                        Index = index,
                    }
                },
            };
        }

        public List<int> GetTableIndexes(Table table)
        {
            var indexes = new List<int>();
            foreach (var row in table.TableRows)
            {
                foreach (var cell in row.TableCells)
                {
                    var index = cell.StartIndex + 1;
                    indexes.Add(index ?? default);
                }
            }
            return indexes;
        }

        public List<int> GetFirstTableIndexes(GoogleDocument document)
        {
            var table = document.Body.Content.FirstOrDefault(x => x.Table != null).Table;
            var indexes = GetTableIndexes(table);
            return indexes;
        }

        public Request GetMergeCellsRequest((int, int) range, int index, (int, int) location)
        {
            var insertTextRequest = new Request()
            {
                MergeTableCells = new MergeTableCellsRequest()
                {
                    TableRange = new TableRange()
                    {
                        TableCellLocation = new TableCellLocation()
                        {
                            TableStartLocation = new Location()
                            {
                                Index = 1,
                                SegmentId = "1",
                            },
                            RowIndex = 1,
                            ColumnIndex = 1,
                        },
                        ColumnSpan = 2,
                        RowSpan = 2,
                    }
                }
            };

            return insertTextRequest;
        }

        public Request GetBoldStyleUpdateRequest((int, int) range)
        {
            return new Request()
            {
                UpdateTextStyle = new()
                {
                    Fields = "Bold",
                    TextStyle = new TextStyle()
                    {
                        Bold = true,
                    },
                    Range = new GoogleDocsRange()
                    {
                        StartIndex = range.Item1,
                        EndIndex = range.Item2,
                    }
                },
            };
        }

        public Request GetTextStyleUpdateRequest((int, int) range, string url)
        {
            return new Request()
            {
                UpdateTextStyle = new()
                {
                    Fields = "Link",
                    TextStyle = new TextStyle()
                    {
                        Link = new Link() { Url = url }
                    },
                    Range = new GoogleDocsRange()
                    {
                        StartIndex = range.Item1,
                        EndIndex = range.Item2,
                    }
                },
            };
        }

        public void ClearDocument(string docId)
        {
            var lastIndex = GetDocumentLastIndex(docId);
            ClearDocument(docId, lastIndex);
        }

        public void ClearDocument(string id, int lastEndIndex)
        {
            var batchUpdateRequest = new BatchUpdateDocumentRequest();
            var deleteContentRequest = new Request();
            if (lastEndIndex == 1)
            {
                return;
            }
            deleteContentRequest.DeleteContentRange = new DeleteContentRangeRequest()
            {
                Range = new GoogleDocsRange()
                {
                    StartIndex = 1,
                    EndIndex = lastEndIndex,
                }
            };

            var deleteParagraphBullets = new Request();
            deleteParagraphBullets.DeleteParagraphBullets = new DeleteParagraphBulletsRequest()
            {
                Range = new GoogleDocsRange()
                {
                    StartIndex = 1,
                    EndIndex = 4,
                }
            };

            batchUpdateRequest.Requests = new List<Request>()
            {
                deleteContentRequest,
            };

            var result = service.Documents.BatchUpdate(batchUpdateRequest, id).Execute();
        }

        public List<string> GetDocumentTextLines(string id)
        {
            var request = service.Documents.Get(id);
            var document = request.Execute();
            var lines = GetLines(document);
            return lines;
        }

        public GoogleDocument GetDocument(string id)
        {
            var request = service.Documents.Get(id);
            GoogleDocument? document = request.Execute();
            return document;
        }

        public List<string> GetDocumentText(string id)
        {
            var document = GetDocument(id);
            var lines = GetLines(document);
            var text = ConcatLines(lines, string.Empty);
            return lines;
        }

        public List<string> GetDocumentText(GoogleDocument document)
        {
            var lines = GetLines(document);
            var text = ConcatLines(lines, string.Empty);
            return lines;
        }

        private List<string> GetLines(GoogleDocument document)
        {
            var content = document.Body.Content;
            var paragraphs = content.Where(x => x.Paragraph != null).Select(x => x.Paragraph).ToList();
            var lines = paragraphs.Where(x => x.Elements != null).SelectMany(x => x.Elements).Select(x => x.TextRun.Content).ToList();

            return lines;
        }

        private string ConcatLines(List<string> lines, string newLine)
        {
            var result = string.Join(newLine, lines) + newLine;
            return result;
        }
    }
}
