using SharpPdfServiceProg.Repetition;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PdfService
{
   public class Program
   {
      static void Main(string[] args)
      {
         var gg = new Dictionary<string, List<string>>();

            var pdfService = OutBorder.PdfService();

         var gg3 = pdfService.Open(@"C:\Users\pawel\Downloads\Doc1.pdf");

         //var slash = '/';
         //var filePath = GetMyDebugProjectPath() + slash + "Test.pdf";
         //var dataAccess = new HeaderDataAccess();

         //var templatePath = pdfService.GetTemplatePathByName("GridOfferTemplate");

         //var offer = new OfferForInverters(dataAccess, filePath)
         //{
         //   TemplatePath = templatePath,
         //};

         //var success = pdfService.Export(offer.DataAccess.GetDummyRows(4), offer.FilePathOutput);

         //System.Diagnostics.Process.Start("cmd.exe ", $"/c {filePath}");
      }

      private static string GetMyDebugProjectPath()
      {
         var myProjectDirectoryName = Assembly.GetCallingAssembly().GetName().Name;
         var up = @"..\";

         var currentDirectoryPath = Directory.GetCurrentDirectory();
         var folderName = Path.GetFileName(currentDirectoryPath);
         while (folderName != myProjectDirectoryName)
         {
            currentDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectoryPath, up));
            folderName = Path.GetFileName(Path.GetDirectoryName(currentDirectoryPath));
         }

         var result = currentDirectoryPath.Replace('\\', '/');

         if (result[result.Length - 1] == '/')
         {
            result = result.Remove(result.Length - 1);
         }

         return result;
      }
   }
}
