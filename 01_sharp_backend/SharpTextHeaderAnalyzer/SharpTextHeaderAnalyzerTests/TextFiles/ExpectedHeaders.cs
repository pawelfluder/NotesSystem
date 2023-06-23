using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextHeaderAnalyzerFrameProj;

namespace TextHeaderAnalyzerCoreTestsProj.TextFiles
{
   public static class ExpectedHeaders
   {
      private static string up = @"..\";
      private static string slash = @"\";
      private static string txtExt = ".txt";
      private static string newLine = Environment.NewLine;

      public static string GetTextFileContent(string name)
      {
         var contentLines = GetTextFileContentLines(name);
         var content = string.Join(newLine, contentLines);

         return content;
      }

      public static string[] GetTextFileContentLines(string name)
      {
         var testRootDirectory = FindUpperDirectoryPath("TextFiles");

            //AppDomain.CurrentDomain.BaseDirectory.Split("bin")[0];
            //var testRootDirectory = Directory.GetParent(TestContext..DeploymentDirectory.TestDirectory).Parent.FullName;
            //string solution_dir = Path.GetDirectoryName(Path.GetDirectoryName(

         var textFileToTestPath = testRootDirectory + @"\" + name + txtExt;
         var lines = File.ReadAllLines(textFileToTestPath);
         var result = lines.Skip(4);

         return result.ToArray();
      }

      private static string FindUpperDirectoryPath(string folderToFindName)
      {
         var currentDirectoryPath = Directory.GetCurrentDirectory();
         var currentFolderName = Path.GetFileName(currentDirectoryPath);
         var currentDirectorySubDirectoriesPaths = Directory.GetDirectories(currentDirectoryPath);

         var currentDirectorySubDirectoriesNames =
            currentDirectorySubDirectoriesPaths.Select(x => Path.GetFileName(Path.GetDirectoryName(x)));

         while (!(currentFolderName == folderToFindName || currentDirectorySubDirectoriesNames.Any(x => x == folderToFindName)))
         {
            currentDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectoryPath, up));
            currentFolderName = Path.GetFileName(Path.GetDirectoryName(currentDirectoryPath));

            currentDirectorySubDirectoriesPaths = Directory.GetDirectories(currentDirectoryPath);
            currentDirectorySubDirectoriesNames =
               currentDirectorySubDirectoriesPaths.Select(x => Path.GetFileName(x));
         }

         if (currentFolderName == folderToFindName)
         {
            return currentDirectoryPath;
         }

         var result = currentDirectorySubDirectoriesPaths.First(x =>
            Path.GetFileName(Path.GetFileName(x)) == folderToFindName);

         return result;
      }


      public static List<Header> GetForFileName(string name)
      {
         var result = new List<Header>();

         switch (name)
         {
            case "TextFile04":
               GetResult4(result);
               break;
            case "TextFile03":
               GetResult3(result);
               break;
            case "TextFile02":
               GetResult2(result);
               break;
            case "TextFile01":
               GetResult1(result);
               break;
         }

         return result;
      }

      private static void GetResult4(List<Header> result)
      {
         result.AddRange(new List<Header>()
            {
                new Header(
                name:"19_11_24",
                subHeaders:new List<Header>()
                {
                    new Header(
                    name:"191124_1025",
                    subHeaders:new List<Header>()
                    {
                        new Header(
                        name: "r; 19_11_24_pf_Justyna_Pietras",
                        content: new List<string>()
                        {
                            "00:13:20",
                            "00:40:40",
                        }),
                    }),

                    new Header(
                    name:"191124_1025",
                    subHeaders:new List<Header>()
                    {
                        new Header(
                        name: "r; 19_11_24_pn_Ola_18_lat",
                        content: new List<string>()
                        {
                            "00:53:47",
                            "00:59:30",
                        }),

                        new Header(
                        name: "r; 19_11_24_pn_Sylwia_zaklad",
                        content: new List<string>()
                        {
                            "01:04:50",
                            "01:13:17",
                        }),
                    }),
                }),
            });
      }

      private static void GetResult1(List<Header> result)
      {
         result.AddRange(new List<Header>()
                    {
                        new Header("Usefull", new List<string>()
                        {
                            "Are we collected?",
                        }),
                    });
      }

      private static void GetResult2(List<Header> result)
      {
         result.AddRange(new List<Header>()
                    {
                        new Header("tittle", new List<string>()
                        {
                            "rebuildplugin",
                        }),
                        new Header("delete plugin folders and packages", new List<string>()
                        {
                            "rmdir /s /Q \"D:\\git\\NGT_Source\\Source\\PluginReferenceImpl\\IEDPlugin\\.vs\"",
                            "rmdir /s /Q \"D:\\GIT\\NGT_Source\\Source\\PluginReferenceImpl\\IEDPlugin\\bin\"",
                            "cd /d \"D:\\GIT\\NGT_Source\\Source\\PluginReferenceImpl\\IEDPlugin\"",
                            "CleanupPackages",
                        }),
                        new Header("restore plugin nuget packages", new List<string>()
                        {
                            "D:\\UseFullFiles\\nuget.exe restore",
                        }),
                    });
      }

      private static void GetResult3(List<Header> result)
      {
         result.AddRange(new List<Header>()
            {
                new Header(
                    name:"Settings",
                    content:new List<string>(),
                    subHeaders:new List<Header>()
                    {
                        new Header(
                            name:"Tittle",
                            content:new List<string>()
                            {
                                "Main site",
                            },
                            subHeaders:new List<Header>()
                            ),
                        new Header(
                            name:"App",
                            content:new List<string>()
                            {
                                "Custom",
                            },
                            subHeaders:new List<Header>()
                        ),
                    }),
            });
      }
   }
}
