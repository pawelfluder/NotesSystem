namespace SharpArgsManagerProj.AAPublic;

public interface IArgsManagerService
{
    string Resolve(string[] args);
}