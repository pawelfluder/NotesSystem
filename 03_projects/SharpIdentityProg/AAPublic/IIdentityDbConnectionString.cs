namespace SharpIdentityProg.AAPublic;

public interface IIdentityDbConnectionString
{
    string GetConnStr();
    string GetDbFilePath();
    string GetDbFolderPath();
    string GetDbFileName();
}
