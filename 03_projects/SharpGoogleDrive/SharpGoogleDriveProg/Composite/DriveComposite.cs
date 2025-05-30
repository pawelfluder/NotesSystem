﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using SharpGoogleDriveProg.Service;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using DriveFile = Google.Apis.Drive.v3.Data.File;
using File = System.IO.File;

namespace SharpGoogleDriveProg.Composite;

// google examples
// https://github.com/LindaLawton/Google-Dotnet-Samples/blob/master/Samples/Drive%20API/v3/FilesSample.cs
public class DriveComposite
{
    private readonly IOperationsService operationsService;
    private readonly GoogleDriveService parentService;
    private DriveService service;
    private readonly IGoogleCredentialWorker credentials;
    private (string Id, string Name) tempFolder;

    public DriveComposite(
        GoogleDriveService parentService,
        DriveService service,
        IOperationsService operationsService)
    {
        this.operationsService = operationsService;
        this.parentService = parentService;
        this.service = service;
        this.credentials = operationsService.Credentials;
    }

    public DriveFile GetFolderByNameAndId(string name, string id)
    {
        var files = GetFilesRequest($"name='{name}'");
        var file = files.Single(x => x.Id == id);
        return file;
    }

    public string CreateDocFile(string title)
    {
        var templateName = "EmbeddedResources.Template04.docx";
        var assemblyName = credentials.GetAssemblyName(this);
        var stream = credentials.GetEmbeddedResourceStream(assemblyName, templateName);
        (var docId, var outTitle) = UploadFileDocx(title, stream);
        return docId;
    }

    public Permission AddReadPermissionForAnyone(string fileId)
    {
        try
        {
            var permission = new Permission
            {
                Type = "anyone",
                Role = "reader",
            };

            var request = service.Permissions.Create(permission, fileId);
            var result = request.Execute();

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }

        return null;
    }

    public (string Id, string Name) GetFileByName(string name)
    {
        var files = GetFilesRequest($"name='{name}'");
        files = files.OrderByDescending(x => x.CreatedTime).ToList();
        var file = files.First();
        var result = (file.Id, file.Name);
        return result;
    }

    public List<(string Id, string Name)> GetSpreadSheetsList(List<string> idsList)
    {
        var query = "mimeType = 'application/vnd.google-apps.spreadsheet'";
        var allFiles = GetFilesRequest(query);
        var tmp = allFiles.Where(x => idsList.Contains(x.Id)).ToList();
        var tmp2 = tmp.Select(x => (x.Id, x.Name)).ToList();
        return tmp2;
    }

    // https://stackoverflow.com/questions/59677223/search-file-inside-shared-drive-by-id-file-using-the-query-filter
    // https://developers.google.com/drive/api/guides/search-files
    public (string Id, string Name) GetFileById(string fileId)
    {
        var request = service.Files.Get(fileId);
        //request.Fields = "*";
        request.Fields = "id, name";
        var file = request.Execute();
        var result = (file.Id, file.Name);
        return result;
    }

    public (string id, string name) UploadTempPhotoFile(Stream fileStream)
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

    public string UploadTempPhotoFile((string folderPath, string fileName) folderQfile)
    {
        var photoFilePath = Path.Combine(folderQfile.folderPath, folderQfile.fileName);
        photoFilePath = photoFilePath.Replace("\\", "/");
        var photoFileStream = File.Open(photoFilePath, FileMode.Open);
        var idQname = UploadTempPhotoFile(photoFileStream);
        var uri = $"https://drive.google.com/u/0/uc?id={idQname.Item1}&export=download";
        return uri;
    }
        
    public void DoPhotoActionAndRemove(
        (string folderPath, string fileName) photoFilePath,
        Action<string> photoAction)
    {
        var uri = UploadTempPhotoFile(photoFilePath);
        var photoId = GetIdFromUri(uri);
            
        photoAction.Invoke(uri);
        RemoveFile(photoId);
    }

    public string GetIdFromUri(string uri)
    {
        string pattern = @"https:\/\/drive.google.com\/u\/0\/uc\?id=([^&]+)&export=download";
        Match match = Regex.Match(uri, pattern);
        if (match.Success)
        {
            string id = match.Groups[1].Value;
            return id;
        }

        throw new InvalidOperationException();
    }
        
    static string ParseName(string arg) {
        var regex = new Regex(@"^OU=([a-zA-Z\\]+\,\s+[a-zA-Z\\]+)\,.*$");
        var match = regex.Match(arg);
        return match.Groups[1].Value;
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

    public (string Id, string Name) CreateNewDocFile(
        string parent,
        string fileName)
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

    public (string, string) UploadFileDocx(
        string fileName,
        string docTemplateStrategy)
    {
        var embededFilePath = "EmbeddedResources." + docTemplateStrategy + ".docx";
        var assemblyName = operationsService.Credentials.GetAssemblyName(this);
        var stream = credentials.GetEmbeddedResourceStream(assemblyName, embededFilePath);
        (string Id, string Name) IdQName = UploadFileDocx(fileName, stream);
        return IdQName;
    }

    public (string, string) UploadFileDocx(
        string fileName,
        Stream fileStream)
    {
        var googleDocument = "application/vnd.google-apps.document";

        var fileMetadata = new DriveFile() // please note this is Google.Apis.Drive.v3.Data.File;
        {
            Name = fileName,
            MimeType = googleDocument
        };

        var request2 = service.Files.Create(
            fileMetadata,
            fileStream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" // please note this is the mimeType of the source file not the tartget
        );
        request2.Fields = "id";
        var result2 = request2.UploadAsync();
        var gg = result2.Result;


        //var fileMime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //var fileMime = "application/vnd.google-apps.document";

        //var driveFile = new DriveFile();
        //driveFile.Name = fileName;
        //driveFile.MimeType = fileMime;

        //FilesResource.CreateMediaUpload request = service.Files.Create(driveFile, fileStream, fileMime);
        //request.Fields = "id";

        //var response = request.Upload();
        fileStream.DisposeAsync();

        (string Id, string Name) file = GetFileByName(fileName);
        SetAnyoneReadPermission(file.Id);

        return file;
    }

    public (string id, string name) UploadFile(
        Stream fileStream,
        string fileName,
        string fileDescription,
        string fileMime,
        List<string> parents)
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

    public List<Permission> GetFilePermissions(string fileId)
    {
        var permissions = service.Permissions.List(fileId);
        permissions.Fields = "permissions(id,role,type,emailAddress)";

        var permissionList = permissions.Execute().Permissions.ToList();
        return permissionList;
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
            throw new Exception();
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

    public List<DriveFile> GetFilesRequest(
        string query,
        string fields = "*")
    {
        // "nextPageToken, incompleteSearch, kind, files(parents, fileExtension, fullFileExtension, id, name, mimeType, permissions)"
        var listRequest = service.Files.List();
        listRequest.PageSize = 400;
        listRequest.Fields = fields;
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

    //public List<(string Name, string Id)> GetFilesRequest(string query)
    //{
    //    var files = Worker.GetFilesRequest(query);
    //    return files.Select(x => (x.Name, x.Id)).ToList();
    //}

    //public List<(string Name, string Id)> GetFilesRequest(string query)
    //{
    //    var files = worker.GetFilesRequest(query);
    //    return files.Select(x => (x.Name, x.Id)).ToList();
    //}

    //public (string Name, string Id) GetFolderByNameAndId(string name, string id)
    //{
    //    var googleFile = Worker.GetFolderByNameAndId(name, id);
    //    return (googleFile.Name, googleFile.Id);
    //}

    //public List<(string Name, string Id)> GetAllMp3Files()
    //{
    //    var items = Worker.GetFilesRequest($"fileExtension='mp3'");
    //    return items.Select(x => (x.Name, x.Id)).ToList();
    //}

    //public (string, string) UploadTempPhotoFile(Stream fileStream)
    //{
    //    var result = Worker.UploadTempPhotoFile(fileStream);
    //    return result;
    //}
}