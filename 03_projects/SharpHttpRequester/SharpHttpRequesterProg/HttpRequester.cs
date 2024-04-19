using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace SharpTinderApiDataImport
{
    public class HttpRequester
    {
        public JObject InvokePost(
            string organizationUri,
            string projectNameOrId,
            string urlLastPart,
            Dictionary<string, string> headers = default,
            (string type, string pat) authentication = default,
            string mediaType = "application/json",
            object body = default)
        {
            (var client, var url) = PrepareParameters(
                organizationUri,
                projectNameOrId,
                urlLastPart,
                headers,
                authentication,
                mediaType = "application/json");

            string json = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var task01 = client.PostAsync(url, httpContent);

            var jsonResponseObj = GetResponseJsonObj(task01);
            return jsonResponseObj;
        }

        public JObject InvokePatch(
            string organizationUri,
            string projectNameOrId,
            string urlLastPart,
            Dictionary<string, string> headers = default,
            (string type, string pat) authentication = default,
            string mediaType = "application/json",
            object body = default)
        {
            (var client, var url) = PrepareParameters(
                organizationUri,
                projectNameOrId,
                urlLastPart,
                headers,
                authentication,
                mediaType);

            string json = JsonConvert.SerializeObject(body, Formatting.Indented);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var task01 = client.PatchAsync(url, httpContent);

            var jsonResponseObj = GetResponseJsonObj(task01);
            return jsonResponseObj;
        }

        public JObject GetResponseJsonObj(Task<HttpResponseMessage> task)
        {
            JObject jsonBodyObj;

            try
            {
                task.Wait();
                var httpResponseMessage = task.Result;
                httpResponseMessage.EnsureSuccessStatusCode();

                var task02 = httpResponseMessage.Content.ReadAsStringAsync();
                var bodyString = task02.Result;
                jsonBodyObj = JObject.Parse(bodyString);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return jsonBodyObj;
        }

        public JObject InvokeGet(
            string organizationUri,
            string projectNameOrId,
            string urlLastPart,
            Dictionary<string, string> headers = default,
            (string type, string pat) authentication = default,
            string mediaType = "application/json")
        {
            (var client, var url) = PrepareParameters(
                organizationUri,
                projectNameOrId,
                urlLastPart,
                headers,
                authentication,
                mediaType);

            var task01 = client.GetAsync(url);
            task01.Wait();
            var httpResponseMessage = task01.Result;

            httpResponseMessage.EnsureSuccessStatusCode();
            var task02 = httpResponseMessage.Content.ReadAsStringAsync();
            var bodyString = task02.Result;
            var jsonBodyObj = JObject.Parse(bodyString);

            return jsonBodyObj;
        }

        public (HttpClient, string) PrepareParameters(
            string organizationUri,
            string projectNameOrId,
            string urlLastPart,
            Dictionary<string, string> headers,
            (string type, string pat) authentication,
            string mediaType)
        {
            var client = new HttpClient();

            if (mediaType != default)
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(mediaType));
            }

            if (authentication != default)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authentication.type,
                Convert.ToBase64String(
                    ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", authentication.pat))));
            }

            string url = string.Empty;
            if (projectNameOrId != null)
            {
                url = organizationUri + "/" + projectNameOrId + urlLastPart;
            }

            if (projectNameOrId == null)
            {
                url = organizationUri + urlLastPart;
            }

            if (headers != default)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            return (client, url);
        }

        public void TempTryInvokePost()
        {
            // try 00
            //var body2 = JsonConvert.SerializeObject(body, Formatting.Indented);
            //httpContent = new StringContent(body2);
            //var task01 = client.PostAsJsonAsync(url, httpContent);
            //var jsonResponseObj0 = GetResponseJsonObj(task01);
            // try 01
            //httpContent = JsonContent.Create(
            //    body,
            //    typeof(Dictionary<string, string>),
            //    new MediaTypeHeaderValue(mediaType),
            //    new JsonSerializerOptions());
            //task01 = client.PatchAsync(url, httpContent);
            //var jsonResponseObj1 = GetResponseJsonObj(task01);
            // try 02
            //var postRequest = new HttpRequestMessage(HttpMethod.Patch, url)
            //{
            //    Content = JsonContent.Create(body)
            //};
            //task01 = client.SendAsync(postRequest);
            //task01.Wait();
            //var httpResponseMessage = task01.Result;
        }
    }
}
