using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PublicWebApi.Registrations;
using SharpApiArgsProg.AAPublic;
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
        object? jsonObj = JsonSerializer.Deserialize<object>(jsonStr);
        return Ok(jsonObj);
    }

    private void Init()
    {
        if (_isInit) return;
        _backend = MyBorder.OutContainer.Resolve<IBackendService>();
        _isInit = true;
    }
}
