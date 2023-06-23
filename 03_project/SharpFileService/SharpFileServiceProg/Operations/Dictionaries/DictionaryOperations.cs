using System.Collections.Generic;

namespace SharpFileServiceProg.Operations.Dictionaries
{
    public class DictionaryOperations
    {
        public void NewShape01(Dictionary<object, object> dict)
        {
            new NewShape01().Visit(dict);
        }

        public void NewShape02(Dictionary<object, object> dict)
        {
            new NewShape02().Visit(dict);
        }
    }
}
