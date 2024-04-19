using SharpRepoServiceProg.FileOperations;
using SharpRepoServiceProg.Service;
using System.Collections.Generic;
using OutBorder1 = SharpFileServiceProg.AAPublic.OutBorder;

namespace SharpRepoServiceCoreProj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileService = OutBorder1.FileService();
            var repoService = new RepoService(fileService);
            
            //var getAllRepoAddresses = new GetRepoAllAddresses(fileService);

            //var path = "D:\\01_Synchronized\\01_Programming_Files\\0fc7da8d-3466-4964-a24c-dfc0d0fef87c\\Notki";
            //var addresses = getAllRepoAddresses.Visit(path);
        }
    }
}
