using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using GoogleDocument = Google.Apis.Docs.v1.Data.Document;
using GoogleDocsRange = Google.Apis.Docs.v1.Data.Range;
using static Google.Apis.Docs.v1.DocumentsResource;

namespace SharpGoogleDocsProg.Worker
{
    public class StackWorker
    {
        //https://developers.google.com/docs/api/how-tos/format-text
        DocsService service;
        GoogleDocument document;
        string docId;
        private int lastIndex;
        private List<Request> stack;

        public int LastIndex => lastIndex;

        public StackWorker(DocsService service)
        {
            this.service = service;
            stack = new List<Request>();
        }

        public bool ExecuteStack()
        {
            var stack2 = stack.Where(x => x != null).ToList();
            var success = TryExecuteBatchUpdate(stack2);
            ClearStack();
            LoadDocument(docId);
            return success;
        }

        public void LoadDocument(string docId)
        {
            var request = service.Documents.Get(docId);
            var document = request.Execute();
            this.document = document;
            this.docId = document.DocumentId;
            lastIndex = (int)document.Body.Content.Last().EndIndex - 1;

            Paragraph gg;
            SectionBreak gg2;
        }

        public int GetDocumentLastIndex(GoogleDocument document)
        {
            var lastEndIndex = (int)document.Body.Content.Last().EndIndex - 1;
            return lastEndIndex;
        }

        public int GetDocumentLastIndex(string docId)
        {
            var request = service.Documents.Get(docId);
            var document = request.Execute();
            this.document = document;
            this.docId = document.DocumentId;
            lastIndex = (int)document.Body.Content.Last().EndIndex - 1;
            return lastIndex;
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

        public void StackUpdateMarginsRequest(
            (double left, double right) leftRight,
            (double top, double bottom) topBottom)
        {
            var req = GetUpdateMarginsRequest(leftRight, topBottom);
            stack.Add(req);
        }

        public GoogleDocument CreateDocFile(string title)
        {
            var body = new GoogleDocument { Title = title };
            var request = new CreateRequest(service, body);
            var googleDocument = request.Execute();
            return googleDocument;
        }

        private Request GetUpdateMarginsRequest(
            (double left, double right) leftRight,
            (double top, double bottom) topBottom)
        {
            var unit = "PT";
            var request = new Request()
            {
                UpdateDocumentStyle = new UpdateDocumentStyleRequest()
                {
                    Fields = "marginLeft, marginRight, marginTop, marginBottom",
                    DocumentStyle = new DocumentStyle()
                    {
                        MarginLeft = new Dimension()
                        {
                            Unit = unit,
                            Magnitude = leftRight.left,
                        },
                        MarginRight = new Dimension()
                        {
                            Unit = unit,
                            Magnitude = leftRight.right,
                        },
                        MarginTop = new Dimension()
                        {
                            Unit = unit,
                            Magnitude = topBottom.top,
                        },
                        MarginBottom = new Dimension()
                        {
                            Unit = unit,
                            Magnitude = topBottom.bottom,
                        },
                    },
                }
            };

            return request;
        }

        public void StackBoldRequests(List<(int, int)> indexesList)//, string repoName, string repo)
        {
            foreach (var indexes in indexesList)
            {
                var req = GetBoldStyleUpdateRequest(indexes);
                stack.Add(req);
            }
        }

        public BatchUpdateDocumentResponse ExecuteBatchUpdateRequest(BatchUpdateDocumentRequest batchRequest, string id)
        {
            var result = service.Documents.BatchUpdate(batchRequest, id).Execute();
            return result;
        }

        public bool TryExecuteBatchUpdate(List<Request> requestsList)
        {
            var success = TryExecuteBatchUpdate(requestsList, docId, 1);
            return success;
        }

        public bool TryExecuteBatchUpdate(List<Request> requestsList, string id, int maxAttemptCount)
        {
            var attemptCount = 0;
            if (maxAttemptCount >= 1 && requestsList.Count > 0)
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
                        return true;
                    }
                    catch (Exception ex)
                    {
                        attemptCount++;
                    }

                    if (attemptCount >= maxAttemptCount)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Request GetInsertTableRequest((int RowsCount, int ColumnsCount) size)
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

        private Request GetInsertTextRequest(int? lastEndIndex, string text)
        {//??
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
            LoadDocument(id);
            var text = ConcatLines(lines, sep);
            AppendToDocument(text);
        }

        public void AppendToDocument(string text)
        {
            var request = GetInsertTextRequest(GetLastIndex(), text);
            ExecuteBatchUpdate(request, docId);
        }

        public void StackOverrideContentRequest(string text)
        {
            StackDeleteAllContentRequest();

            var req2 = GetInsertTextRequest(1, text);
            stack.Add(req2);
        }

        public void StackAppendTextRequest(string text)
        {
            var req = GetInsertTextRequest(GetLastIndex(), text);
            stack.Add(req);
        }

        public void StackInsertTextRequest(int index, string text)
        {
            if (text != null && text.Count() > 0)
            {
                var req = GetInsertTextRequest(index, text);
                stack.Add(req);
            }

        }

        public void StackInsertBoldTextRequests(int index, string text)
        {
            if (text.Count() > 0)
            {
                var req1 = GetInsertTextRequest(index, text);
                var range = (index, index + text.Length);
                var req2 = GetBoldStyleUpdateRequest(range);
                stack.Add(req1);
                stack.Add(req2);
            }
        }

        public void ClearStack()
        {
            stack = new List<Request>();
        }

        public void StackInsertTableRequest((int RowsCount, int ColumnsCount) size)
        {
            stack.Add(GetInsertTableRequest(size));
        }


        private int? GetLastIndex()
        {
            if (lastIndex == default)
            {
                LoadDocument(docId);
                lastIndex = (int)document.Body.Content.Last().EndIndex - 1;
            }

            return lastIndex;
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

        public List<int> GetFirstTableCellsIndexes()
        {
            var firstTableElement = document.Body.Content.FirstOrDefault(x => x.Table != null);
            if (firstTableElement == null)
            {
                throw new Exception();
            }

            var indexes = GetTableIndexes(firstTableElement.Table);
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

        public int GetFirstTableIndex()
        {
            var firstTableElement = document.Body.Content.FirstOrDefault(x => x.Table != null);
            if (firstTableElement == null)
            {
                throw new Exception();
            }

            var index = firstTableElement.StartIndex;
            return (int)index;
        }

        public void StackMergeCellsRequest((int RowIndex, int ColumnIndex) location, int index, (int RowSpan, int ColumnSpan) range)
        {
            var req = GetMergeCellsRequest(location, index, range);
            stack.Add(req);
        }

        public void StackUpdateTableColumnPropertiesRequest(int index, int with, List<int> columnNumbers)
        {
            var req = GetUpdateTableColumnPropertiesRequest(index, with, columnNumbers);
            stack.Add(req);
        }

        private Request GetUpdateTableColumnPropertiesRequest(int index, int with, List<int> columnNumbers)
        {
            var columnNumbers2 = columnNumbers.Select(x => (int?)x).ToList();
            var request = new Request()
            {
                UpdateTableColumnProperties = new UpdateTableColumnPropertiesRequest()
                {
                    Fields = "width,widthType",
                    ColumnIndices = columnNumbers2,
                    TableStartLocation = new Location()
                    {
                        Index = index,
                    },
                    TableColumnProperties = new TableColumnProperties()
                    {
                        Width = new Dimension
                        {
                            Unit = "PT",
                            Magnitude = with,
                        },
                        WidthType = "FIXED_WIDTH",
                    }
                }
            };

            return request;
        }

        private Request GetMergeCellsRequest((int RowIndex, int ColumnIndex) location, int index, (int RowSpan, int ColumnSpan) range)
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
                                Index = index,
                                //SegmentId = "1",
                            },
                            RowIndex = location.RowIndex - 1,
                            ColumnIndex = location.ColumnIndex - 1,
                        },
                        ColumnSpan = range.ColumnSpan,
                        RowSpan = range.RowSpan,
                    }
                }
            };

            return insertTextRequest;
        }

        public void StackTwoColumnsDocumentRequest()
        {
            var req = GetTwoColumnsDocumentRequest();
            stack.Add(req);
        }

        public Request GetTwoColumnsDocumentRequest()
        {
            var unit = "PT";
            var cpList = new List<SectionColumnProperties>();
            cpList.Add(new SectionColumnProperties()
            {
                PaddingEnd = new Dimension()
                {
                    Magnitude = 14,
                    Unit = unit,
                },
                //Width = new Dimension()
                //{
                //    Magnitude = 1,
                //    Unit = unit,
                //}
            });

            cpList.Add(new SectionColumnProperties()
            {
                PaddingEnd = new Dimension()
                {
                    Magnitude = 0,
                    Unit = unit,
                },
                //Width = new Dimension()
                //{
                //    Magnitude = 15,
                //    Unit = unit,
                //}
            });

            return new Request()
            {
                UpdateSectionStyle = new()
                {
                    Fields = "columnProperties",
                    SectionStyle = new()
                    {
                        ColumnProperties = cpList,
                    },
                    Range = new()
                    {
                        StartIndex = 1,
                        EndIndex = 1// lastIndex,
                    }
                },
            };
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

        public void StackDeleteAllContentRequest()
        {
            var req = GetDeleteAllContentRequest();
            stack.Add(req);
        }

        private Request GetDeleteAllContentRequest()
        {
            var lastIndex = GetLastIndex();
            if (lastIndex == 1)
            {
                return null;
            }

            var deleteContentRequest = new Request()
            {
                DeleteContentRange = new DeleteContentRangeRequest()
                {
                    Range = new GoogleDocsRange()
                    {
                        StartIndex = 1,
                        EndIndex = lastIndex,
                    }
                },
            };

            return deleteContentRequest;
        }

        public List<string> GetDocumentTextLines(string id)
        {
            var request = service.Documents.Get(id);
            var document = request.Execute();
            var lines = GetTextLines(document);
            return lines;
        }

        public List<string> GetDocumentTextLines()
        {
            var lines = GetTextLines(document);
            return lines;
        }

        public string GetDocumentAllText()
        {
            var lines = GetTextLines(document);
            var text = ConcatLines(lines, string.Empty);
            return text;
        }

        public List<string> GetDocumentText(GoogleDocument document)
        {
            var lines = GetTextLines(document);
            var text = ConcatLines(lines, string.Empty);
            return lines;
        }

        private List<string> GetTextLines(GoogleDocument document)
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

        private void Test()
        {
            var deleteParagraphBullets = new Request();
            deleteParagraphBullets.DeleteParagraphBullets = new DeleteParagraphBulletsRequest()
            {
                Range = new GoogleDocsRange()
                {
                    StartIndex = 1,
                    EndIndex = 4,
                }
            };
        }
    }
}
