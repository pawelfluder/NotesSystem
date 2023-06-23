using SharpFileServiceProg.Service;
using System;
namespace SharpFileServiceProg.Repetition
{
    public static class Border
    {
        public static IFileService NewFileService()
        {
            return new FileService();
        }
    }
}
