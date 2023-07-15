using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using GoogleDriveCoreApp;
using GoogleDriveCoreApp.Contract;

namespace SharpGoogleDriveProg.Service
{
    public class GoogleDriveService : IGoogleDriveService
    {
        private DriveService service;
        private readonly string clientId;
        private readonly string clientSecret;

        public DriveWorker Worker { get; private set; }

        public GoogleDriveService(string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            Initialize();
        }

        public void Initialize()
        {
            string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
            string ApplicationName = GetType().Name;

            var secrets = new ClientSecrets();
            secrets.ClientId = clientId;
            secrets.ClientSecret = clientSecret;

            var credentialAuthorization = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                Scopes,
                "user",
                CancellationToken.None);

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentialAuthorization.Result,
                ApplicationName = ApplicationName,
            });

            Worker = new DriveWorker(service);
        }

        public List<(string Name, string Id)> GetFilesRequest(string query)
        {
            var files = Worker.GetFilesRequest(query);
            return files.Select(x => (x.Name, x.Id)).ToList();
        }

        //public List<(string Name, string Id)> GetFilesRequest(string query)
        //{
        //    var files = worker.GetFilesRequest(query);
        //    return files.Select(x => (x.Name, x.Id)).ToList();
        //}

        public (string Name, string Id) GetFolderByNameAndId(string name, string id)
        {
            var googleFile = Worker.GetFolderByNameAndId(name, id);
            return (googleFile.Name, googleFile.Id);
        }

        public List<(string Name, string Id)> GetAllMp3Files()
        {
            var items = Worker.GetFilesRequest($"fileExtension='mp3'");
            return items.Select(x => (x.Name, x.Id)).ToList();
        }

        public (string, string) UploadTempPhotoFile(Stream fileStream)
        {
            var result = Worker.UploadTempPhotoFile(fileStream);
            return result;
        }
    }
}
