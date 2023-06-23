using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleDriveCoreApp.Contract
{
    public interface IGoogleDriveService
    {
        (string Name, string Id) GetFolderByNameAndId(string name, string id);

        List<(string Name, string Id)> GetFilesRequest(string query);

        List<(string Name, string Id)> GetAllMp3Files();
    }
}
