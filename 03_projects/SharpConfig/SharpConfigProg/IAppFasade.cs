using Microsoft.AspNetCore.Builder;
using SharpContainerProg.AAPublic;

namespace SharpConfigProg;

public interface IAppFasade
{
    WebApplicationBuilder Builder { get; }
    WebApplication App { get; }
    IContainer4 Container { get; }
    List<Action<WebApplication>> WebAppActionsList { get; }
    void Run();
}