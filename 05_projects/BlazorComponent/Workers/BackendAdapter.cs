using System.Text;
using System.Text.Json;

namespace BlazorComponent.Workers;

public class BackendAdapter
{
    private readonly HttpClient _client;
    private readonly string _url;

    public BackendAdapter(
        string url)
    {
        _url = url;
        _client = new HttpClient();
    }

    public async Task<HttpResponseMessage> InvokeStringArgsApi(
        params string[] args)
    {
        string json = JsonSerializer.Serialize(args);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpRequestMessage request = new(
            HttpMethod.Patch,
            _url)
        {
            Content = content
        };

        HttpResponseMessage response = await _client
            .SendAsync(request);
        return response;
    }
}
