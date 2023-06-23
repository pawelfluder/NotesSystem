using Google.Apis.Drive.v3;
using System.IO;
using System.Threading.Tasks;


namespace GoogleDriveCoreApp
{
   class Program
   {
      // If modifying these scopes, delete your previously saved credentials
      // at ~/.credentials/drive-dotnet-quickstart.json


      static void Main(string[] args)
      {

         //var googleDriveService = new GoogleDriveService();
         //var allMp3Files = googleDriveService.GetAllMp3Files();



         var mainFolderName = "nagrania";
         //var mainFolder = googleDriveService.GetFolderByName(mainFolderName);



         //var gg2 = googleDriveService.GetAllMp3FilesInFolder(mainFolder);

         // Define parameters of request.

         //listRequest.PageSize = 12;
         //listRequest.Corpora.All(x)


         var nagrania = "1io3zCC_XgAriftC-dNiYGQIzc3TdVqAF";
         var dodatkowe = "1k8tSXrPvY55A9_BIxRPoSxaV3GL20Ut5";
         var excelStatystyki = "1AxC5EG2U3RXDEoPlLahvZkZ83y4L8QUd";
         var root = "root";

         //listRequest.Q = $"'{nagrania}' in parents";



         //1AxC5EG2U3RXDEoPlLahvZkZ83y4L8QUd'

         // List files.



         //var gg = new List<GoogleFile>();
         //foreach (var item in files)
         //{
         //   listRequest.Q = $"'{item.Id}' in parents";
         //   var record = listRequest.Execute().Files;
         //   gg.AddRange(record);
         //}


         //var gg2 = gg.Select(x => x.Name);
         //   Console.WriteLine("Files:");

         //var gg3 = gg2.Count();

         //   if (files != null && files.Count > 0)
         //   {
         //      foreach (var file in files)
         //      {
         //         Console.WriteLine("{0} ({1})", file.Name, file.Id);

         //      Stream outputstream = new MemoryStream();
         //      var request2 = service.Files.Get(file.Id);

         //      request2.Download(outputstream);

         //      outputstreamvar fileStream = Download(file.Id, service);

         //      fileStream.Wait(100);
         //      Stream gg = fileStream.Result;

         //      var extension = file.FullFileExtension;

         //      SaveStreamAsFile("D:\\", outputstream, "temp.xlsx");
         //   }
         //   }
         //   else
         //   {
         //      Console.WriteLine("No files found.");
         //   }
         //   Console.Read();
      }

      public static async Task<Stream> Download(string fileId, DriveService service)
      {
         Stream outputstream = new MemoryStream();
         var request = service.Files.Get(fileId);

         await request.DownloadAsync(outputstream);

         outputstream.Position = 0;

         return outputstream;
      }

      public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
      {
         DirectoryInfo info = new DirectoryInfo(filePath);
         if (!info.Exists)
         {
            info.Create();
         }

         string path = Path.Combine(filePath, fileName);
         using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
         {
            inputStream.CopyTo(outputFileStream);
         }
      }
   }
}