using Google.Apis.Docs.v1.Data;

namespace GoogleDocsServiceProj.Models;

public record ResponseStatus(
    string description,
    BatchUpdateDocumentResponse response = null,
    List<Exception> exceptions = null)
{
}