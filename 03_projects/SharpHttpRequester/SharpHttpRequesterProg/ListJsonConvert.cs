using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpHttpRequesterProg
{
    public static class ListJsonConvert
    {
        public static List<T> DeserializeList<T>(List<JToken> jsonList)
        {
            var objList = new List<T>();
            foreach (var item in jsonList)
            {
                var obj = JsonConvert.DeserializeObject<T>(item.ToString());
                objList.Add(obj);
            }

            return objList;
        }
    }
}
