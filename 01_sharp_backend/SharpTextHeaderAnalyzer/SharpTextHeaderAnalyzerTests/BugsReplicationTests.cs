using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class BugsReplicationTests
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void Bug_20_04_13()
      {
         //Visualization
         //.//level_1
         //. //level_2
         //.  //level_3
         //. //level_2
         //.  //level_3
         //.  //level_3

         //Arrange
         var levelList = new List<int>()
         {
            1, 2, 3, 2, 3, 3
         };

         var expextedParentNumberList = new List<int>()
         {
            0, 1, 2, 1, 4, 4
         };

         //Act
         var correctedLevelList = textAnalyzer.CorrectHeaderLevelList(levelList);
         var parentNumbers = textAnalyzer.GetParentHeaderNumberList(correctedLevelList);

         //Assert
         Assert.IsTrue(expextedParentNumberList.IsStructListEqual(parentNumbers));
      }
   }
}
