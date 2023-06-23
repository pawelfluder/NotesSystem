using System.IO;

namespace TextHeaderAnalyzerFrameProj
{
   class Program
   {
      //Todo
      //Dopisać logikę i testy do omijania i usuwania niepotrzenych tabów i pustych lini

      //Dyskusyjne
      //Czy pomiędzy headerami ma byc nowa linia bez tab'ów? Jak to rozwiązać?
      //Najpierw filtr usuwający tab'y w pustych liniach?

      //Todo
      //Zrobić alias git Update
      //Zaisntalować notepad compare plugin
      //Porównać z HeaderPrinterTests dlaczego jest różnica
      //Zaczac już pisac logike do callStackRecorder
      //Stworzyć już solucję i folder App
      //Wyrówanie w plikach poprawic
      //Visualization expected i Visualization input

      //Stworzyć plik .gitignore id excludować .vs bin i config
      //Napisać StringService - często używane ciągi znaków i znaki
      //Dodać testy do obu solucji zwykłej i core we wspólnym folderze

      //Install-Package NUnit -Version 3.12.0
      //https://social.technet.microsoft.com/wiki/contents/articles/51338.net-core-application-unit-testing-using-mstest.aspx

      static void Main(string[] args)
      {
         TextAnalyzer analyzer = new TextAnalyzer();

         var path = GetMyProjectsInputPath();

         var headers = analyzer.AnalyzeFile(path);


      }

      private static string GetMyProjectsInputPath()
      {
         var up = @"..\";
         var slash = @"\";
         var myProjectDirectoryName = "01_Moje-projekty";

         var currentDirectoryPath = Directory.GetCurrentDirectory();
         var folderName = Path.GetFileName(currentDirectoryPath);
         while (folderName != myProjectDirectoryName)
         {
            currentDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectoryPath, up));
            folderName = Path.GetFileName(Path.GetDirectoryName(currentDirectoryPath));
         }

         var inputDirectoryName = "Input";
         var inputFileName = "Input.txt";

         return currentDirectoryPath + inputDirectoryName + slash + inputFileName;
      }
   }
}
