using BackendAdapters.Names;
using BackendAdapters.Workers;

namespace BackendAdaptersTests;

public class Tests
{
    // 
    [Test]
    public async Task RepoAdapter_Method_GetAllReposNames()
    {
        // Arrange
        HttpClient httpClient = new();
        BackendAdapter backend = new(httpClient);
        RepoAdapter repo = new RepoAdapter(backend);

        // Act
        var result = await repo
            .GetAllReposNames();
        
        // Assert
        Console.WriteLine("Result: " + result);
    }
    
    [Test]
    public async Task RepoAdapter_Item_GetByGuid()
    {
        // Arrange
        HttpClient httpClient = new();
        BackendAdapter backend = new(httpClient);
        RepoAdapter repo = new RepoAdapter(backend);

        // Act
        var result = await repo.GetByGuid("Notki", Guid.Parse("5752ee48-47b9-4f6a-9fc2-62b6d8d421eb"));
        
        // Assert
        Console.WriteLine("Result: " + result);
    }
    
    [Test]
    public async Task RepoAdapter_GetFirstRepo()
    {
        // Arrange
        HttpClient httpClient = new();
        BackendAdapter backend = new(httpClient);
        RepoAdapter repo = new RepoAdapter(backend);

        // Act
        (string, string) item = await repo.GetFirstRepo();
        
        // Assert
        Console.WriteLine("Result: " + item);
    }
    
    [Test]
    public async Task RepoAdapter_GetItem()
    {
        // Arrange
        HttpClient httpClient = new();
        BackendAdapter backend = new(httpClient);
        RepoAdapter repo = new RepoAdapter(backend);

        // Act
        var item = await repo.GetItem(("Notki", ""));
        
        // Assert
        Console.WriteLine("Response: " + item);
    }
    
    [Test]
    public async Task BackendAdapter_GetItem()
    {
        // Arrange
        string url = "http://localhost:5056/strArgsApi";
        HttpClient httpClient = new();
        BackendAdapter backend = new(httpClient, url);

        // Act
        string itemJson = await backend.InvokeStringArgsApi(
        [SNames.RepoService, SNames.ItemWorker, SNames.GetItem,
            "Notki",
            ""]);
        
        // Assert
        Assert.IsNotNull(itemJson);
        Console.WriteLine("Response: " + itemJson);
    }
}
