using System.IO;
using System;
using System.Collections.Generic;

namespace SharpFileServiceProg.Service
{
    public partial interface IFileService
    {
        public interface IVisit
        {
            public void Visit(
                string path,
                Action<FileInfo> fileAction,
                Action<DirectoryInfo> directoryAction);
        }

        public interface IVisit<T>
        {
            public T Visit(
                string path);
        }

        public interface IParentVisit
        {
            public List<DirectoryInfo> Parents { get; }

            public void Visit(
                string path,
                Action<FileInfo> fileAction,
                Action<DirectoryInfo> directoryAction);
        }


        public interface IYamlOperations
        {
            string Serialize(object input);
            string SerializeToFile(string filePath, object input);

            object Deserialize(string yamlText);
            object DeserializeFile(string path);

            T Deserialize<T>(string yamlText);
            T DeserializeFile<T>(string path);

            bool TryDeserialize<T>(string yamlText, out T result);
        }
    }
}
