using Google.Apis.Docs.v1.Data;
using GoogleDocsServiceProj.Models;
using SharpGoogleDocsProg.Composits;
using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.AAPublic;

public class ExecuteComposite
{
    private readonly RequestsCoposite _requestsComposite;
    private readonly DocumentComposite _documentCoposite;

    public ExecuteComposite(
        RequestsCoposite requestsComposite,
        DocumentComposite documentCoposite)
    {
        _documentCoposite = documentCoposite;
        _requestsComposite = requestsComposite;
    }
    
    public void InsertPhotoToFirstTableTop(int width, string uri)
    {
        var index = 1;
        InsertPhotoToFirstTable(width, uri, index);
    }
    
    public void InsertPhotoToBottomOfFirstTable(int width, string uri)
    {
        // todo - check if reload is not needed
        // _documentCoposite.ReLoadDocument();
        var lastIndex = _documentCoposite.GetLastIndexOfFirstCellOfFistTable();
        ResponseStatus status = InsertPhotoToFirstTable(width, uri, lastIndex);
    }
    
    private ResponseStatus InsertPhotoToFirstTable(int width, string uri, int index)
    {
        var photoRequests = new List<Request>();
        var request = _requestsComposite.GetInsertPhotosRequest(width, uri, index);
        photoRequests.Add(request);
        var status = _requestsComposite.ExecuteRequest(request);
        return status;
    }
}