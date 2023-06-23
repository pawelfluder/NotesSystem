using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerCoreTestsProj.TextFiles;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class HeaderPrinterTests
   {
      private HeaderPrinter headerPrinter;

      [TestInitialize]
      public void Setup()
      {
         headerPrinter = new HeaderPrinter();
      }

      [TestMethod]
      [DataRow("TextFile01")]
      [DataRow("TextFile02")]
      [DataRow("TextFile03")]
      [DataRow("TextFile04")]
      public void Get1(string fileName)
      {
         // Arrange
         var input = ExpectedHeaders.GetForFileName(fileName);
         var expected = ExpectedHeaders.GetTextFileContent(fileName);

         // Act
         //var printed = headerPrinter.Print(input);

         // Assert
         //Assert.AreEqual(expected, printed);
      }
   }
}