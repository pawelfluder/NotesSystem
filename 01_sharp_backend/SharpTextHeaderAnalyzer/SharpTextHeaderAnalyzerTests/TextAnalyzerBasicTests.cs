using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerBasicTests
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void Get1()
      {
         //Arrange
         var inputLines = new[]
         {
                "//Start",
                "   Content of start",
            };
         var inputList = new List<int>()
            {
                1,
            };
         var expectedHeaderList = new List<Header>()
            {
                new Header(name: "Start",
                    content: new List<string>()
                    {
                        "Content of start",
                    }),
            };

         //Act
         var outputHeaderList = textAnalyzer.GetHeaders(inputLines, inputList);

         //Asset
         Assert.IsTrue(expectedHeaderList.IsClassListEqual(outputHeaderList));
      }

      [TestMethod]
      public void Get2()
      {
         //Arrange
         var inputLines = new[]
         {
                "//Start",
                "   Content of start",
                "//Stop",
                "   Content of stop",
            };
         var inputDictionary = new List<int>()
            {
                1,
                3,
            };
         var expectedHeaderList = new List<Header>()
            {
                new Header(name: "Start",
                    content: new List<string>()
                    {
                        "Content of start",
                    }),
                new Header(name: "Stop",
                    content: new List<string>()
                    {
                        "Content of stop",
                    }),
            };

         //Act
         var outputHeaderList = textAnalyzer.GetHeaders(inputLines, inputDictionary);

         //Asset
         Assert.IsTrue(expectedHeaderList.IsClassListEqual(outputHeaderList));
      }

      [TestMethod]
      public void Get3()
      {
         //Arrange
         var inputLines = new[]
         {
                "//Start",
                "	Content of start",
                "	//Stop",
                "		Content of stop",
            };
         var inputDictionary = new List<int>()
            {
                1,
                3,
            };
         var expectedHeaderList = new List<Header>()
            {
                new Header(name: "Start",
                    content: new List<string>()
                    {
                        "Content of start",
                    },subHeaders:new List<Header>()
                    {
                        new Header(name: "Stop",
                            content: new List<string>()
                            {
                                "Content of stop",
                            }),
                    }),
            };

         //Act
         var outputHeaderList = textAnalyzer.FindHeaders(inputLines);

         //Asset
         Assert.IsTrue(expectedHeaderList.IsClassListEqual(outputHeaderList));
      }
   }
}
