using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerCoreProj;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerYamlTests
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void TextAnalyzerTest1()
      {
         // Arrange
         string Document = @"---
         american:
             - Boston Red Sox
         american:
             - Detroit Tigers
         american:
             - New York Yankees
         ...".RemYamlBeginSpaces();

         var expectedHeader = new Header(
         "amer;ican",
         new List<string>()
         {
            "Boston Red Sox",
            "Detroit Tigers",
            "New York Yankees"
         });

         // Act
         object obj = YamlUtils.ToExpando(Document);
         var outputHeader = textAnalyzer.ToHeader(obj as ExpandoObject);

         // Assert
         Assert.AreEqual(expectedHeader, outputHeader);
      }
   }
}
