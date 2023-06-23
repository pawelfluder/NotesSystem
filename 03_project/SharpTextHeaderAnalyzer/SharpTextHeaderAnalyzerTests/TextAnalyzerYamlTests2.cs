using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerCoreProj;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerYamlTests2
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void TextAnalyzerTest2()
      {
         // Arrange
         string document = @"---
          - Boston Red Sox
          - Detroit Tigers
          - New York Yankees
         ...".RemYamlBeginSpaces();

         var expectedHeader = new Header(
         "",
         new List<string>()
         {
            "Boston Red Sox",
            "Detroit Tigers",
            "New York Yankees"
         });

         // Act
         var obj = YamlUtils.ToExpando(document);
         var outputHeader = textAnalyzer.ToHeader(obj);

         // Assert
         Assert.AreEqual(expectedHeader, outputHeader);
      }
   }
}
