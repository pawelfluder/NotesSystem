using System.IO;
using System;

namespace SharpFileServiceProg.Service
{
    public partial class FileService2
    {
        private string penPath = @"E:\RECORDER";
        private string nameFileString = "nazwa.txt";
        private string contentFileString = "lista.txt";

        //public ServerInfo Info { get; private set; }

        private string rootPath;
        private string searchPlace;

        private string rootLocalHttp = "http://localhost:8080/";

        private string slash = "/";
    }
}
