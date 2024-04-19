using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ceTe.DynamicPDF.Printing;
using PdfService.Offer;
using PdfService.PdfService;
using PdfService.Worker;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace SharpPdfServiceProg.Service
{
    public class PdfExecutor2 : IPdfService2
    {

        public bool Export(
            List<(string type, int level, string text)> rows,
            string outputPath)
        {
            try
            {
                //GlobalFontSettings.FontResolver = new FontResolver();
                var document = new PdfDocument();
                var pdfContainer = new PdfContainer(document);

                AddFromAllWorkers(rows, pdfContainer);
                document.Save(outputPath);
                document.Close();
            }
            catch (Exception ex)
            {
                return false;
            }

            var readed = PdfReader.TestPdfFile(outputPath);
            var success = readed != 0;
            return success;
        }

        public PdfDocument OpenPdf(string path)
        {
            var pdf = PdfReader.Open(path);
            return pdf;
        }

        public void RunPrinter2(string pdfFilePath)
        {
            var printers = Printer.GetLocalPrinters();
            var driverName = "Brother HL-1210W series";
            var printer = printers.SingleOrDefault(x => x.DriverName == driverName);
            PrintJob printJob = new PrintJob(driverName, pdfFilePath);
            printJob.Print();
        }

        public void RunPrinter(string pdfFilePath)
        {
            var driverName = "Brother HL-1210W series";
            PrintUsingAdobeAcrobat(pdfFilePath, driverName);
        }

        public string GetTemplatePathByName(string templateName)
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory() + '/' + "Template";

            var files = Directory.GetFiles(currentDirectoryPath).Select(x => ToSlash(x));

            var template = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x) == templateName);

            return template;
        }

        private string ToSlash(string input)
        {
            return input.Replace('\\', '/');
        }

        private void AddFromAllWorkers(
            List<(string type, int level, string text)> rows,
            PdfContainer pdfContainer)
        {
            pdfContainer.NewPage();

            //var companyPageWorker = new CompanyPageWoker();
            //companyPageWorker.Generate(offer, pdfContainer);

            var gridWorker = new HeaderGridWorker2();
            gridWorker.Generate(rows, pdfContainer);
        }

        private string GetMyDebugProjectPath()
        {
            var myProjectDirectoryName = Assembly.GetCallingAssembly().GetName().Name;
            var up = @"..\";

            var currentDirectoryPath = Directory.GetCurrentDirectory();
            var folderName = Path.GetFileName(currentDirectoryPath);
            while (folderName != myProjectDirectoryName)
            {
                currentDirectoryPath = Path.GetFullPath(Path.Combine(currentDirectoryPath, up));
                folderName = Path.GetFileName(Path.GetDirectoryName(currentDirectoryPath));
            }

            return currentDirectoryPath;
        }

        public bool Open(string path)
        {
            try
            {

                var adobreExePath = "C:\\Program Files\\Adobe\\Acrobat DC\\Acrobat\\Acrobat.exe";
                var workingDirectory = Directory.GetParent(path).FullName;
                var fileName = Path.GetFileName(path);
                var process = new ProcessStartInfo();
                process.FileName = adobreExePath;
                process.Arguments = path;
                Process.Start(process);
                return true;
            }
            catch
            {
                return false;
            }            
        }

        public static void PrintUsingAdobeAcrobat(string fullFilePathForPrintProcess, string printerName)
        {
            string printApplicationPath = Microsoft.Win32.Registry.LocalMachine
            .OpenSubKey("Software")
            .OpenSubKey("Microsoft")
            .OpenSubKey("Windows")
            .OpenSubKey("CurrentVersion")
            .OpenSubKey("App Paths")
            .OpenSubKey("Acrobat.exe")
            .GetValue(String.Empty).ToString();

            const string flagNoSplashScreen = "/s";
            const string flagOpenMinimized = "/h";

            var flagPrintFileToPrinter = string.Format("/t \"{0}\" \"{1}\"", fullFilePathForPrintProcess, printerName);

            var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);

            var startInfo = new ProcessStartInfo
            {
                FileName = printApplicationPath,
                Arguments = args,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            var process = Process.Start(startInfo);
            process.EnableRaisingEvents = true;

            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.WaitForExit();
                    process.WaitForInputIdle();
                    process.CloseMainWindow();
                }

                process.Kill();
                process.Dispose();
            }
        }

        public void PrintUsingSumatraPDF(string fullFilePathForPrintProcess, string printerName, CancellationToken cancellationToken)
        {
            var gg = new SumatraPDFWrapper.SumatraPDFWrapper();
            gg.Print(fullFilePathForPrintProcess, printerName);

            //PrintQueue print_queue = LocalPrintServer.GetDefaultPrintQueue();

            //Process print = new Process();
            //print.StartInfo.FileName = "sumatrapdf.exe";
            //print.StartInfo.UseShellExecute = true;
            //print.StartInfo.CreateNoWindow = true;
            //print.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //print.StartInfo.Arguments = "-print-to \"" + printerName + "\" -exit-when-done \"" + fullFilePathForPrintProcess + "\"";
            //print.Start();
            
            //PrintSystemJobInfo target_job = null;
            //while (!print.HasExited && target_job == null && !cancellationToken.IsCancellationRequested)
            //{
            //    print_queue.Refresh();
            //    PrintJobInfoCollection info = print_queue.GetPrintJobInfoCollection();
            //    foreach (PrintSystemJobInfo job in info)
            //    {
            //        if (job.Name.Substring(job.Name.LastIndexOf("\\") + 1) == fullFilePathForPrintProcess)
            //        {
            //            target_job = job;
            //        }
            //    }
            //    Thread.Sleep(50);
            //}

            //while (target_job != null && !target_job.IsCompleted && !cancellationToken.IsCancellationRequested)
            //{
            //    Thread.Sleep(50);
            //}
        }
    }
}
