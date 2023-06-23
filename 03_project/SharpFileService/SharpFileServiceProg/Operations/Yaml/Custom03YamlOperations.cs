using System;
using System.IO;
using SharpFileServiceProg.Service;
using YamlDotNet.Serialization;

namespace SharpFileServiceProg.Operations.Yaml
{
    internal class Custom03YamlOperations : IFileService.IYamlOperations
    {
        private readonly IDeserializer custom03Deserializer;
        private readonly ISerializer custom03Serializer;

        public Custom03YamlOperations()
        {
            var builder = new DeserializerBuilder();
            custom03Deserializer = builder.Build();

            var builder2 = new SerializerBuilder().
                WithEventEmitter(next => new QuotedScalarEventEmitter(next));
                //WithEventEmitter(next => new QuotedValueEmitter(next));
            //.JsonCompatible();
            custom03Serializer = builder2.Build();
        }

        public string Serialize(object input)
        {
            try
            {
                var result = custom03Serializer.Serialize(input);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public string SerializeToFile(string filePath, object input)
        {
            try
            {
                var result = custom03Serializer.Serialize(input);
                File.WriteAllText(filePath, result);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public object Deserialize(string yamlText)
        {
            try
            {
                var result = custom03Deserializer.Deserialize<object>(yamlText);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public object DeserializeFile(string path)
        {
            try
            {
                var yamlText = File.ReadAllText(path);
                var result = custom03Deserializer.Deserialize<object>(yamlText);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public T Deserialize<T>(string yamlText)
        {
            try
            {
                var result = custom03Deserializer.Deserialize<T>(yamlText);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public T DeserializeFile<T>(string path)
        {
            try
            {
                var yamlText = File.ReadAllText(path);
                var result = custom03Deserializer.Deserialize<T>(yamlText);
                return result;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        private void HandleError(Exception ex)
        {
            throw ex;
        }

        public bool TryDeserialize<T>(string yamlText, out T result)
        {
            throw new NotImplementedException();
        }
    }
}
