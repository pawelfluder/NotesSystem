using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerCoreTestsProj.TextFiles;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerTests
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      [DataRow("TextFile01")]
      [DataRow("TextFile02")]
      [DataRow("TextFile03")]
      [DataRow("TextFile04")]
      public void TextAnalyzerTest1(string fileName)
      {
         //Arrange
         var lines = ExpectedHeaders.GetTextFileContentLines(fileName);
         var expectedHeaders = ExpectedHeaders.GetForFileName(fileName);

         //Act
         var analyzerHeaders = textAnalyzer.FindHeaders(lines);

         //Assert
         Assert.IsTrue(expectedHeaders.IsClassListEqual(analyzerHeaders));

         var printer = new HeaderPrinter();
         //var e = printer.Print(expectedHeaders);
         //var o = printer.Print(analyzerHeaders);

      }
   }
}
