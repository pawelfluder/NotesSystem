using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class CorrectLevelList
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void ExampleOne()
      {
         // Visualization input
         //.  //level_3
         //. //level_2
         //.//level_1
         //.   //level_3

         // Visualization expected
         //.//level_1
         //.//level_1
         //.//level_1
         //.   //level_3

         //Arrange
         var levelList = new List<int>()
            {
                3, 2, 1, 3,
            };

         var expectedCorrectedLevelList = new List<int>()
            {
                1, 1, 1, 3,
            };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);

         //Assert
         Assert.IsTrue(expectedCorrectedLevelList.IsStructListEqual(correctedLevelList));
      }

      [TestMethod]
      public void ExampleTwo()
      {
         //Visualization
         //.   //level_4
         //.  //level_3
         //. //level_2

         //Arrange
         var levelList = new List<int>()
            {
                4, 3, 2,
            };

         var expectedCorrectedLevelList = new List<int>()
            {
                1, 1, 1,
            };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);

         //Assert
         Assert.IsTrue(expectedCorrectedLevelList.IsStructListEqual(correctedLevelList));
      }

      [TestMethod]
      public void ExampleThree()
      {
         //Visualization
         //.   //level_4
         //.  //level_3
         //.   //level_4

         //Arrange
         var levelList = new List<int>()
            {
                4, 3, 4,
            };

         var expectedCorrectedLevelList = new List<int>()
            {
                1, 1, 2,
            };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);

         //Assert
         Assert.IsTrue(expectedCorrectedLevelList.IsStructListEqual(correctedLevelList));
      }
   }
}
