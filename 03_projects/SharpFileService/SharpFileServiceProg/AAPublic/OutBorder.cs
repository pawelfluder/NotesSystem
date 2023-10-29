using SharpFileServiceProg.Service;
using System;
namespace SharpFileServiceProg.AAPublic
{
    public static class OutBorder
    {
        public static IFileService FileService()
        {
            return new FileService();
        }
    }
}
