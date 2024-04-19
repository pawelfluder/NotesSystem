using System;
using System.Collections.Generic;

namespace SharpFileServiceProg.Operations.Dictionaries
{
    public class NewShape01
    {
        public void Visit(Dictionary<object, object> dict)
        {
            new RecursivelyLeaveOnlyInDictionaries(new List<string>()
            {
                "photos",
                "url",

                "id",
                "name",
                "birth_date",
                "bio",

                "person",
                "messages",
                "message",
                "from",
                "sent_date",
            }).Visit(dict);

            new RecursivelyMoveInDictionaries(new Dictionary<string, int>
            {
                { "bio", 1 },
                { "birth_date", 1 },
                { "name", 1 },
            }).Visit(dict);

            new RecursivelyMoveSelectedInDictionaries(new List<(string, string, int)>
            {
                ("photos", "url", 2 ),
            }).Visit(dict);

            new RecursivelyRemoveFromDictionaries(new List<string>()
            {
                "person",
            }).Visit(dict);

            new RecursivelyCheckRequiredKeysInDictionaries(new List<(string, string)>
            {
                ("root", "url"),
                ("root", "id"),
                ("root", "name"),
                ("root", "birth_date"),
                ("root", "bio"),
                ("root", "messages"),
                ("message", "sent_date"),
                ("message", "from"),
            }).Visit(dict);

            new RecursivelyNullToDefaultInDictionaries(new List<(string, object)>
            {
                ("bio", string.Empty),
            })
            .Visit(dict);
        }
    }
}
