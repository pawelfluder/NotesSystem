using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpYaml.Serialization;
using TextHeaderAnalyzerCoreProj;
using TextHeaderAnalyzerFrameProj;
using YamlDotNet.Serialization;
using SerializerSharYaml = SharpYaml.Serialization.Serializer;
using SerializerYamlDotNet = YamlDotNet.Serialization.Serializer;

namespace TextHeaderAnalyzerCoreTestsProj
{
   [TestClass]
   public class TextAnalyzerYamlTests4
   {
      private TextAnalyzer textAnalyzer;

      [TestInitialize]
      public void Setup()
      {
         textAnalyzer = new TextAnalyzer();
      }

      [TestMethod]
      public void TextAnalyzerTest4()
      {
         // Arrange
         

         string document7 = @"---
          - 580:
             - |
               fb
               20-08-15
               pf
               Gabriela
               Górska
               x
               20-08-15_pf_Gabriela_Górska
               20-08-15_pf
               Gabriela_Górska
         ...".RemYamlBeginSpaces();

         string document8 = @"---
          - Id: 580
            Contact: fb
            Date: 20-08-15
            Type: pf
            Name: Gabriela
            Surname: Górska
            Atributes: x
         ...".RemYamlBeginSpaces();

         var labels = new List<string>()
         {
            "Kontakt",
            "Dzieñ",
            "Rodzaj",
            "Imiê",
            "Nazwisko",
            "Atrybuty",
            "Nazwa",
            "Who1",
            "Who2",
         };

         // Act
         var nodes = YamlUtils.ReadYamlNodes(document8);

         //deserializer.Serialize(nodes,)

         var serializer = new SerializerSharYaml();
         var serializer2 = new SerializerYamlDotNet();

         //var config = (ApproachData)deserializer.Deserialize(document8, typeof(ApproachData[]));

         

         var deserializer2 = new DeserializerBuilder().Build();
         object result = serializer.Deserialize<object>(document8);
         var result2 = serializer.Deserialize<List<DataApproach>>(document8);

         var gg9 = serializer2.Serialize(result);

         var settings = new SerializerContextSettings();
         string gg3 = serializer.Serialize(result2);
         string gg2 = serializer.Serialize(result);

         var obj1 = YamlUtils.ToExpando(document8);

         var gg = obj1 as List<object>;

         dynamic ff = gg.ElementAt(0);
         
         

         //obj1.fe
      }
   }
}
