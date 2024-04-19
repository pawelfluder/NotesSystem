using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using System;
using System.IO;
using NewtonJsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SharpFileServiceProg.Operations.Yaml
{
    internal class ByJsonYamlOperations : IFileService.IYamlOperations
    {
        private NewtonJsonSerializer jsonSerializer;

        public ByJsonYamlOperations()
        {
            jsonSerializer = new NewtonJsonSerializer();
        }

        public string Serialize(object input)
        {
            throw new NotImplementedException();
        }

        public string SerializeToFile(string filePath, object input)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(string yamlText)
        {
            try
            {
                var w = new StringWriter();
                var js = new NewtonJsonSerializer();
                js.Serialize(w, yamlText);
                string jsonText = w.ToString();
                var jObj = JsonConvert.DeserializeObject<object>(jsonText);
                return jObj;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public object DeserializeFile(string path)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string yamlText)
        {
            try
            {
                var w = new StringWriter();
                var js = new NewtonJsonSerializer();
                js.Serialize(w, yamlText);
                string jsonText = w.ToString();
                var jObj = JsonConvert.DeserializeObject<T>(jsonText);
                return jObj;
            }
            catch (Exception ex)
            {
                HandleError(ex);
                return default;
            }
        }

        public T DeserializeFile<T>(string path)
        {
            throw new NotImplementedException();
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
