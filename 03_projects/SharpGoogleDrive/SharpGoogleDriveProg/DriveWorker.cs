using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DriveFile = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveCoreApp
{
    public class DriveWorker
    {
        private DriveService service;
        private (string Id, string Name) tempFolder;

        public DriveWorker(DriveService service)
        {
            this.service = service;
        }

        public DriveFile GetFolderByNameAndId(string name, string id)
        {
            var files = GetFilesRequest($"name='{name}'");
            var file = files.Single(x => x.Id == id);

            return file;
        }

        public (string, string) UploadTempPhotoFile(Stream fileStream)
        {
            var guid = Guid.NewGuid();
            var fileName = "temp-" + guid + ".jpg";
            var description = string.Empty;
            var fileMime = "image/jpg";
            var tempFolder = GetTempFolder();
            var parents = new List<string> { tempFolder.Id };

            var result = UploadFile(fileStream, fileName, description, fileMime, parents);
            return result;
        }

        public void DeleteTempFolder()
        {
            RemoveFile(tempFolder.Id);
        }

        private (string Id, string Name) GetTempFolder()
        {
            if (tempFolder == default)
            {
                tempFolder = CreateFolder(null, "temp");
            }

            return tempFolder;
        }

        public (string Id, string Name) CreateFolder(string parent, string fileName)
        {
            var fileMime = "application/vnd.google-apps.folder";

            var driveFile = new DriveFile();
            driveFile.Name = fileName; // fileName;
            driveFile.MimeType = fileMime;
            driveFile.PermissionIds = new List<string> { "anyoneWithLink" };
            driveFile.Parents = new List<string>() { };

            var request = service.Files.Create(driveFile);
            request.Fields = "id, name";

            var response = request.Execute();
            var result = (response.Id, response.Name);
            return result;
        }

        public (string Id, string Name) CreateNewDocFile(string parent, string fileName)
        {
            var guid = Guid.NewGuid();
            var description = string.Empty;
            //var fileMime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            //var fileMime = "application/vnd.oasis.opendocument.text";
            //var fileMime = "application/x-vnd.oasis.opendocument.spreadsheet";
            var fileMime = "application/vnd.google-apps.document";

            var driveFile = new DriveFile();
            driveFile.Name = fileName; // fileName;
            driveFile.MimeType = fileMime;
            driveFile.PermissionIds = new List<string> { "anyoneWithLink" };
            driveFile.Parents = new List<string>() { };

            var request = service.Files.Create(driveFile);
            request.Fields = "id, name";

            var response = request.Execute();
            var result = (response.Id, response.Name);
            return result;
        }

        public void UploadFile(string fileName, string fileDescription, string fileMime, string folder)
        {
            var driveFile = new DriveFile();
            driveFile.Name = fileName; // fileName;
            driveFile.PermissionIds = new List<string> { "anyoneWithLink" };

            var request = service.Files.Create(driveFile);
            request.Fields = "id";

            var response = request.Execute();
        }

        public (string, string) UploadFile(Stream fileStream, string fileName, string fileDescription, string fileMime, List<string> parents)
        {
            var driveFile = new DriveFile();
            driveFile.Name = fileName; // fileName;
            driveFile.Description = fileDescription; //fileDescription;
            driveFile.MimeType = fileMime;// fileMime;
            driveFile.PermissionIds = new List<string> { "anyoneWithLink" };
            driveFile.Parents = parents;

            var request = service.Files.Create(driveFile, fileStream, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            var gg4 = Path.GetFileNameWithoutExtension(fileName);
            var query = $"name = '{fileName}'";
            var files = GetFilesRequest(query);
            var file = files.First();
            //var file = (gg.id)
            var result = (file.Id, file.Name);

            SetAnyoneReadPermission(file.Id);

            return result;
        }

        public List<Permission> GetAnyOnePermissions()
        {
            var permission = new Permission();
            permission.Kind = "drive#permission";
            permission.Id = "anyoneWithLink";
            permission.Role = "writer";
            permission.AllowFileDiscovery = false;
            var permissionsList = new List<Permission>() { permission };
            return permissionsList;
        }

        public void SetAnyoneReadPermission(string fileId)
        {
            var permission = new Permission();
            permission.Role = "reader";
            permission.Role = "writer";
            permission.Type = "anyone";

            var ur = service.Permissions.Create(permission, fileId);
            var res = ur.Execute();

            if (res.Type != "anyone")
            {
                throw new System.Exception();
            }
        }

        public void RemoveFile(string fileId)
        {
            var ur = service.Files.Delete(fileId);
            ur.Execute();
        }

        public void RemoveFiles(List<string> fileIdLIst)
        {
            foreach (var fileId in fileIdLIst)
            {
                RemoveFile(fileId);
            }
        }

        public List<DriveFile> GetFilesRequest(string query)
        {
            var listRequest = service.Files.List();
            listRequest.PageSize = 400;
            listRequest.Fields = "nextPageToken, incompleteSearch, kind, files(parents, fileExtension, fullFileExtension, id, name, mimeType, permissions)";
            listRequest.Q = query;
            var result = listRequest.Execute();
            var files = result.Files.ToList();

            while (result.NextPageToken != null)
            {
                listRequest.PageToken = result.NextPageToken;
                result = listRequest.Execute();
                files.AddRange(result.Files);
            }

            return files;
        }

        private IList<DriveFile> GetAllMp3FilesInFolder(DriveFile file)
        {
            var items = GetFilesRequest($"'{file.Id}' in parents");
            var listOfGoogleFiles = new List<DriveFile>();

            foreach (var item in items)
            {
                if (item.FileExtension == null)
                {
                    listOfGoogleFiles.AddRange(GetAllMp3FilesInFolder(item));
                }
                else if (item.FileExtension == "mp3")
                {
                    listOfGoogleFiles.Add(item);
                }
            }

            return items;
        }
    }
}
