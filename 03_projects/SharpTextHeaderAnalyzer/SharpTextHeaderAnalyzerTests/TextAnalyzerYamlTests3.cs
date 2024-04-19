using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextHeaderAnalyzerCoreProj;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerYamlTests3
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void TextAnalyzerTest3()
      {
         // Arrange
         string document = @"---
         jobs:
          - job: A
          - job: B
         ...".RemYamlBeginSpaces();

         string document2 = @"---
          - jobs:
             - job:
               echo
             - job: B
               steps:
                - bash: echo B
         ...".RemYamlBeginSpaces();

         string document3 = @"---
         hr: 65    # Home runs
         avg: 0.278 # Batting average
         rbi: 147   # Runs Batted In
         ...".RemYamlBeginSpaces();

         string document4 = @"---
         american:
         - Boston Red Sox
            - Detroit Tigers
            - New York Yankees
         national:
         - New York Mets
            - Chicago Cubs
            - Atlanta Braves
         ...".RemYamlBeginSpaces();


         string document5 = @"---
         name: name-a1
          - name-a2
          - name-a3
         content:
          - line-a1
          - line-a2
          - name: name-b1
            content:
             - line-b1
             - name:
               content:
          - line-a3
         ...".RemYamlBeginSpaces();

         string document6 = @"---
          - name-a1; name-a2:
             - line-la1
             - name-b2:
                - line-lb2
         ...".RemYamlBeginSpaces();

         string document7 = @"---
          - name-a1:
             - |
               line1
               line2
             - name-b2:
                - line-lb2
         ...".RemYamlBeginSpaces();



         // Act
         var obj1 = YamlUtils.ToExpando(document6);
         var obj = YamlUtils.ToExpando(document4);
      }
   }
}
