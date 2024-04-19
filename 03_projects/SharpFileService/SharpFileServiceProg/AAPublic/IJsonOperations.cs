namespace SharpFileServiceProg.AAPublic
{
    public interface IJsonOperations
    {
        T DeserializeObject<T>(string jsonString);
        T TryDeserializeObject<T>(string jsonString);
        string SerializeObject(object obj);
        string TrySerializeObject(object obj);
    }
}