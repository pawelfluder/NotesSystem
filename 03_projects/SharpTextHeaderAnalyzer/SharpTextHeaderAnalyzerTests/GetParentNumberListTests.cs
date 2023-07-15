using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class GetParentNumberListTests
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void ExampleFive()
      {
         //Visualization
         //.//level_1
         //. //level_2
         //.  //level_3
         //. //level_2

         //Arrange
         var levelList = new List<int>()
            {
                1, 2, 3, 2,
            };

         var expextedParentNumberList = new List<int>()
            {
                0, 1, 2, 1,
            };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(correctedLevelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }

      [TestMethod]
      public void ExampleFour()
      {
         //Visualization
         //.  //level_3
         //. //level_2
         //.//level_1
         //.   //level_3

         //Arrange
         var levelList = new List<int>()
            {
                3, 2, 1, 3,
            };

         var expextedParentNumberList = new List<int>()
            {
                0, 0, 0, 3,
            };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(correctedLevelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }


      [TestMethod]
      public void ExampleThree()
      {
         //Visualization
         //.//level_1
         //.//level_1
         //.//level_1

         //Arrange
         var levelList = new List<int>()
            {
                1, 1, 1,
            };

         var expextedParentNumberList = new List<int>()
            {
                0, 0, 0,
            };

         //Act
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(levelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }


      [TestMethod]
      public void ExampleTwo()
      {
         //Visualization
         //.//1
         //. //2
         //.//1
         //.   //3

         //Arrange
         var levelList = new List<int>()
            {
                1,2,1,3
            };

         var expextedParentNumberList = new List<int>()
            {
                0,1,0,3,
            };

         //Act
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(levelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }

      [TestMethod]
      public void ExampleOne()
      {
         //Visualization
         //.//1
         //. //2
         //.  //3

         //Arrange
         var levelList = new List<int>()
            {
                1,2,3
            };

         var expextedParentNumberList = new List<int>()
            {
                0, 1, 2,
            };

         //Act
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(levelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }
   }
}
