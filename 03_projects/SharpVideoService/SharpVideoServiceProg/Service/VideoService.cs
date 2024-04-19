using FFMpegCore;
using SharpFileServiceProg.Service;
using SharpVideoServiceProg.AAPublic;

namespace SharpVideoServiceProg.Service
{
    // https://www.nuget.org/packages/Xabe.FFMpeg/2.2.3
    // https://github.com/tomaszzmuda/Xabe.FFmpeg

    internal class VideoService : IVideoService
    {
        private IFileService fileService;
        private string nugetGlobalFolderPath;
        private bool initialized;

        public VideoService(IFileService fileService)
        {
            this.fileService = fileService;
            this.nugetGlobalFolderPath = GetNugetGlobalFolderPath();
            initialized = false;
        }

        public async Task<bool> PosterWithAudio(
            string imageFilePath,
            string audioFilePath,
            string outputVideoFilePath)
        {
            await Initialize();
            if (!initialized) { return false; }
            var success = await Task.Run(() => FFMpegPosterWithAudio(imageFilePath, audioFilePath, outputVideoFilePath));
            return success;
        }

        public async ValueTask<bool> Initialize()
        {
            if (initialized) { return true; }

            if (!TryCopyAssemblies())
            {
                initialized = true;
                return initialized;
            }

            var temporaryFolder = GlobalFFOptions.Current.TemporaryFilesFolder;
            GlobalFFOptions.Configure(new FFOptions { BinaryFolder = nugetGlobalFolderPath, TemporaryFilesFolder = temporaryFolder });
            initialized = true;
            return initialized;
        }

        private bool FFMpegPosterWithAudio(string imageFilePath, string audioFilePath, string outputVideoFilePath)
        {
            try
            {
                FFMpeg.PosterWithAudio(imageFilePath, audioFilePath, outputVideoFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetNugetGlobalFolderPath()
        {
            var pathWithVariable = "%USERPROFILE%/.nuget/packages";
            var result = Environment.ExpandEnvironmentVariables(pathWithVariable).Replace("\\", "/");
            return result;
        }

        private bool TryCopyAssemblies()
        {
            var fileName1 = "ffmpeg.exe";
            var fileName2 = "ffprobe.exe";

            var outputFilePath1 = nugetGlobalFolderPath + "/" + fileName1;
            var outputFilePath2 = nugetGlobalFolderPath + "/" + fileName2;

            if (File.Exists(outputFilePath1) &&
                File.Exists(outputFilePath2))
            {
                return true;
            }

            var searchFolderName = "02_assemblies";
            var expression = "5(2,2)";
            try
            {
                var folderPath = fileService.Path
                .FindFolder(searchFolderName, Environment.CurrentDirectory, expression);
                var inputFilePath1 = folderPath + "/" + fileName1;
                var inputFilePath2 = folderPath + "/" + fileName2;

                File.Copy(inputFilePath1, outputFilePath1, true);
                File.Copy(inputFilePath2, outputFilePath2, true);
                return true;
            }
            catch
            {
                return false;
            }            
        }
    }
}
