namespace SharpIdentityProg.AAPublic;

public interface IIdentityDbConnectionProvider
{
    string GetConnStr();
    string GetDbFilePath();
    string GetDbFolderPath();
    string DbFileName { get; }
}
