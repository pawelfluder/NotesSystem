using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using System.Collections.Generic;

namespace SharpRepoServiceProg.Repetition
{
    public class OutBorder
    {
        public static IRepoService RepoService(
            IFileService fileService)
        {
            return new RepoService(fileService);
        }
    }
}
