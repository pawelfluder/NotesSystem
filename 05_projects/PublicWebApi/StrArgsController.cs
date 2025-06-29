using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PublicWebApi.Registrations;
using SharpRepoBackendProg.Services;

namespace PublicWebApi;

[ApiController]
[Route("[controller]")]
public class StrArgsApiController : ControllerBase
{
    private IBackendService _backend;
    private bool _isInit;

    [HttpPatch]
    [Route("")]
    public IActionResult PatchArgs(
        [FromBody] string[] args)
    {
        Init();
        string jsonStr = _backend.InvokeStringArgsApi(args);
        if (string.IsNullOrEmpty(jsonStr))
        {
            return Ok("");
        }

        Type type = GetDeserializeType(jsonStr);
        object? jsonObj = JsonSerializer.Deserialize(jsonStr, type);
        return Ok(jsonObj);
    }

    private Type GetDeserializeType(
        string jsonStr)
    {
        bool s01 = bool.TryParse(jsonStr, out bool boolResult);
        if (s01) return typeof(bool);

        return typeof(object);
    }

    private void Init()
    {
        if (_isInit) return;
        _backend = MyBorder.OutContainer.Resolve<IBackendService>();
        _isInit = true;
    }
}
