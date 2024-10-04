namespace SharpOperationsProg.AAPublic.Operations;

public interface IYamlWrk
{
    IYamlOperations Dotnet { get; }
    IYamlOperations Sharp { get; }
    IYamlOperations Byjson { get; }
    IYamlOperations Custom01 { get; }
    IYamlOperations Custom02 { get; }
    IYamlOperations Custom03 { get; }
}