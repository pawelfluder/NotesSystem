using System.Globalization;
using System.Text;
using System.Text.Json;

namespace BackendAdapters.Workers;

public class BackendAdapter
{
    private HttpClient _client;
    private readonly string _url;

    public BackendAdapter(
        HttpClient client,
        string url = null)
    {
        _url = url ?? "http://localhost:5056/strArgsApi";;
        _client = client;
        _client.BaseAddress = new Uri(_url);
    }

    public async Task<string> InvokeStringArgsApi(
        params string[] args)
    {
        string json = JsonSerializer.Serialize(args);
        StringContent content = new(
            json,
            Encoding.UTF8, 
            "application/json");

        HttpRequestMessage request = new(
            HttpMethod.Patch,
            _url)
        {
            Content = content
        };
        
        //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        HttpResponseMessage response = await _client
            .SendAsync(request);
        string responseJson = await response.Content.ReadAsStringAsync();
        return responseJson;
    }
}
