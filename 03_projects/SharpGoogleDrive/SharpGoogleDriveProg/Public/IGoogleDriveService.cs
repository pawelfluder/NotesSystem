using GoogleDriveCoreApp;

namespace SharpGoogleDriveProg.Public
{
    public interface IGoogleDriveService
    {
        (string Name, string Id) GetFolderByNameAndId(string name, string id);

        List<(string Name, string Id)> GetFilesRequest(string query);

        List<(string Name, string Id)> GetAllMp3Files();

        public DriveWorker Worker { get; }
    }
}
