using SharpApiArgsProg;
using SharpApiArgsProg.Services;

namespace SharpArgsManagerProj.AAPublic;

public class OutBorder
{
    public static IArgsManagerService ArgsManagerService()
    {
        var service = new ApiArgsService();
        return service;
    }
}