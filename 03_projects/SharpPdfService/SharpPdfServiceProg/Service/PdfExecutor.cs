//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using PdfService.GridWorker;
//using PdfService.Offer;
//using PdfService.Worker;
//using PdfSharpCore.Pdf;
//using PdfSharpCore.Pdf.IO;

//namespace SharpPdfServiceProg.Service
//{
//    public class PdfExecutorOld : IPdfServiceOld
//    {
//        public bool Export(List<IRow> rows, string outputPath)
//        {
//            try
//            {
//                //GlobalFontSettings.FontResolver = new FontResolver();
//                var document = new PdfDocument();
//                var pdfContainer = new PdfContainer(document);

//                AddFromAllWorkers(rows, pdfContainer);
//                document.Save(outputPath);
//                document.Close();

//            }
//            catch (Exception ex)
//            {
//                // Log error - document already open by process
//                // Log error - draw outside of the canvas

//                return false;
//            }



//            var readed = PdfReader.TestPdfFile(outputPath);
//            var success = readed != 0;
//            return success;
//        }

//        public PdfDocument OpenPdf(string path)
//        {
//            var pdf = PdfReader.Open(path);
//            return pdf;
//        }

//        public string GetTemplatePathByName(string templateName)
//        {
//            var currentDirectoryPath = Directory.GetCurrentDirectory() + '/' + "Template";

//            var files = Directory.GetFiles(currentDirectoryPath).Select(x => ToSlash(x));

//            var template = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == templateName);

//            return template;
//        }

//        private string ToSlash(string input)
//        {
//            return input.Replace('\\', '/');
//        }

//        private void AddFromAllWorkers(List<IRow> rows, PdfContainer pdfContainer)
//        {
//            pdfContainer.NewPage();

//            //var companyPageWorker = new CompanyPageWoker();
//            //companyPageWorker.Generate(offer, pdfContainer);

//            var gridWorker = new HeaderGridWorker();
//            gridWorker.Generate(rows, pdfContainer);
//        }

//        private string GetMyDebugProjectPath()
//        {
//            var myProjectDirectoryName = Assembly.GetCallingAssembly().GetName().Name;
//            var up = @"..\";

//            var currentDirectoryPath = Directory.GetCurrentDirectory();
//            var folderName = Path.GetFileName(currentDirectoryPath);
//            while (folderName != myProjectDirectoryName)
//            {
//                currentDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectoryPath, up));
//                folderName = Path.GetFileName(Path.GetDirectoryName(currentDirectoryPath));
//            }

//            return currentDirectoryPath;
//        }

//        public bool Open(string path)
//        {
//            try
//            {
//                PdfReader.Open(path);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}
