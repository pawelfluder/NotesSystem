using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfService.Offer;
using RepoServiceCoreProj.Service;
using SharpConfigProg.Service;
using TextHeaderAnalyzerCoreProj.Service;

namespace PdfServiceTests
{
    [TestClass]
    public class PdfServiceTest
    {
        [TestMethod]
        public void Given_()
        {
            var headerNotesService = new HeaderNotesService();

            // Arrange
            var slash = @"\";
            var filePath = GetMyDebugProjectPath() + slash + "Test.pdf";
            var pdfService = new PdfService.PdfService.PdfExecutor2();

            var configService = new ConfigService();
            configService.PrepareOnlyRepoPaths();
            var repoRootPaths = (configService.Paths["repoRootPaths"] as List<object>)
                    .Select(x => x.ToString()).ToList();
            var repoService = new RepoService(repoRootPaths);

            var repo = "Notki";
            var loca = "01/02";
            var text = repoService.Methods.ReadText((repo, loca));

            var textLines = repoService.Methods.ReadTextLines((repo, loca));
            var name = repoService.Methods.GetLocalName((repo, loca));
            var elementsList = headerNotesService.GetElements2(textLines.Skip(4).ToArray());

            var pdfCreated = pdfService.Export(elementsList, filePath);

            // Assert
            Assert.AreEqual(true, pdfCreated);

            // Open pdf file
            //System.Diagnostics.Process.Start(filePath);
        }

        private string GetMyDebugProjectPath()
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

            return currentDirectoryPath;
        }
    }
}
