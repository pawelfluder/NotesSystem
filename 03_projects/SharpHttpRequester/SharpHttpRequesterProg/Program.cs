using Newtonsoft.Json;

namespace SharpTinderApiDataImport
{
    public static class Program
    {
        public static void Main()
        {
            //var organizationUri1 = "https://dev.azure.com/MvpProjects";
            //var projectNameOrId = "FirstMvp";
            //var pat = "";
            //var urlRequestPart01b = "/_apis/build/builds?definitions=8&queryOrder=queueTimeDescending&api-version=6.0";

            //var urlRequestPart02 = "/_apis/build/builds/220?api-version=7.0";
            //var urlRequestPart03 = "/_apis/projects";

            //var jsonBodyObj01b = test.GetProjects(organizationUri1, projectNameOrId, pat, urlRequestPart01);
            //var gg1 = jsonBodyObj01.First;

            //var value = jsonBodyObj01["value"].ToList();
            //var tmp1 = value[0].ToString();

            //var r = JsonConvert.DeserializeObject<Build>(tmp1);


            ////test.GetProjects(organizationUri1, null, pat, urlRequestPart03);

            ////test.GetProjects(organizationUri1, null, pat, urlRequestPart03);

            //var jsonBodyObj02 = test.GetProjects(organizationUri1, projectNameOrId, pat, urlRequestPart02);

            //var buildNumber = jsonBodyObj02["buildNumber"].ToString();
            //var buildId = jsonBodyObj02["id"].ToString();
        }
    }
}



//         private static void Write(this RootObject rootObject)
//        {
//            Console.WriteLine("clientId: " + rootObject.args.clientId);
//            Console.WriteLine("Accept: " + rootObject.headers.Accept);
//            Console.WriteLine("AcceptEncoding: " + rootObject.headers.AcceptEncoding);
//            Console.WriteLine("AcceptLanguage: " + rootObject.headers.AcceptLanguage);
//            Console.WriteLine("Authorization: " + rootObject.headers.Authorization);
//            Console.WriteLine("Connection: " + rootObject.headers.Connection);
//            Console.WriteLine("Dnt: " + rootObject.headers.Dnt);
//            Console.WriteLine("Host: " + rootObject.headers.Host);
//            Console.WriteLine("Origin: " + rootObject.headers.Origin);
//            Console.WriteLine("Referer: " + rootObject.headers.Referer);
//            Console.WriteLine("UserAgent: " + rootObject.headers.UserAgent);
//            Console.WriteLine("origin: " + rootObject.origin);
//            Console.WriteLine("url: " + rootObject.url);
//            Console.WriteLine("data: " + rootObject.data);
//            Console.WriteLine("files: ");
//            foreach (KeyValuePair<string, string> kvp in rootObject.files ?? Enumerable.Empty<KeyValuePair<string, string>>())
//            {
//                Console.WriteLine("\t" + kvp.Key + ": " + kvp.Value);
//            }
//        }
//    }

//    public class Args
//    {
//        public string ClientId { get; set; }
//    }

//    public class Headers
//    {
//        public string Accept { get; set; }

//        public string AcceptEncoding { get; set; }

//        public string AcceptLanguage { get; set; }

//        public string Authorization { get; set; }

//        public string Connection { get; set; }

//        public string Dnt { get; set; }

//        public string Host { get; set; }

//        public string Origin { get; set; }

//        public string Referer { get; set; }

//        public string UserAgent { get; set; }
//    }

//    public class RootObject
//    {
//        public Args args { get; set; }

//        public Headers Headers { get; set; }

//        public string Origin { get; set; }

//        public string Url { get; set; }

//        public string Data { get; set; }

//        public Dictionary<string, string> Files { get; set; }
//    }
//}