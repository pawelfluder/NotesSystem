using SharpFileServiceProg.Service;
using System;
namespace SharpFileServiceProg.Repetition
{
    public static class OutBorder
    {
        public static IFileService FileService()
        {
            return new FileService();
        }
    }
}
