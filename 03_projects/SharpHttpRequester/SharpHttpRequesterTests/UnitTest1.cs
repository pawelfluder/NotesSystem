using Newtonsoft.Json;
using SharpHttpRequesterProg;
using SharpHttpRequesterTests.JsonObjects;
using SharpTinderApiDataImport;
using SharpTinderApiDataImport.JsonObjects;

namespace SharpHttpRequesterTests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //azureDevopsPat
        }

        

        [TestMethod]
        public void GetAllBuilds()
        {
            // arrange
            var httpRequester = new HttpRequester();
            var authenticationType = "Basic";
            var organizationUri = "https://dev.azure.com/MvpProjects";
            var projectNameOrId = "FirstMvp";
            var pat = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var urlRequestPart = "/_apis/build/builds?definitions=8&queryOrder=queueTimeDescending&api-version=6.0";

            // act
            var jsonBodyObj = httpRequester.InvokeGet(
                organizationUri,
                projectNameOrId,
                urlRequestPart,
                default,
                (authenticationType, pat));

            // arrange
            var value = jsonBodyObj["value"].ToList();
            var firstBuildJObj = value.First();
            var firstBuild = JsonConvert.DeserializeObject<Build>(firstBuildJObj.ToString());
            var builds = ListJsonConvert.DeserializeList<Build>(value);
        }

        [TestMethod]
        public void QueueBuild()
        {
            // arrange
            var httpRequester = new HttpRequester();
            var authenticationType = "Basic";
            var organizationUri = "https://dev.azure.com/MvpProjects";
            var projectNameOrId = "FirstMvp";
            var pat = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var buildNumber = 260;
            //https://dev.azure.com/MvpProjects/FirstMvp/_apis/build/builds/260?api-version=6.0
            var urlRequestPart = $"/_apis/build/builds?api-version=6.0";
            var body = new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "definition", new Dictionary<string, string>()
                    {
                        { "id", "8" }
                    }
                }
            };

            // act
            var jsonBodyObj = httpRequester.InvokePost(
                organizationUri,
                projectNameOrId,
                urlRequestPart,
                default,
                (authenticationType, pat),
                "application/json",
                body);

            // arrange
            var value = jsonBodyObj["value"].ToList();
            var firstBuildJObj = value.First();
            var firstBuild = JsonConvert.DeserializeObject<Build>(firstBuildJObj.ToString());
            var builds = ListJsonConvert.DeserializeList<Build>(value);
        }

        [TestMethod]
        public void UpdateBuildNumber()
        {
            // arrange
            var httpRequester = new HttpRequester();
            var authenticationType = "Basic";
            var organizationUri = "https://dev.azure.com/MvpProjects";
            var projectNameOrId = "FirstMvp";
            var pat = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var buildNumber = 260;
            //https://dev.azure.com/MvpProjects/FirstMvp/_apis/build/builds/260?api-version=6.0
            var urlRequestPart = $"/_apis/build/builds/{buildNumber}?api-version=6.0";
            var body1 = new Dictionary<string, string>() { { "buildNumber", "1.2.3.4" } };
            //var body2 = new DI("buildNumber", "1.2.3.4.start");
            var body = new Dictionary<string, string>()
            {
                { "buildNumber", "1.2.3.4.start" }
            };
            //JsonConvert.SerializeObject(body1, Formatting.Indented);

            // act
            var jsonBodyObj = httpRequester.InvokePatch(
                organizationUri,
                projectNameOrId,
                urlRequestPart,
                default,
                (authenticationType, pat),
                "application/json",
                body);

            // arrange
            var build = JsonConvert.DeserializeObject<Build>(jsonBodyObj.ToString());
        }

        [TestMethod]
        public void GetOneBuild()
        {
            // arrange
            var httpRequester = new HttpRequester();
            var authenticationType = "Basic";
            var organizationUri = "https://dev.azure.com/MvpProjects";
            var projectNameOrId = "FirstMvp";
            var pat = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var urlRequestPart = "/_apis/build/builds/220?api-version=7.0";

            // act
            var jsonBodyObj = httpRequester.InvokeGet(
                organizationUri,
                projectNameOrId,
                urlRequestPart,
                default,
                (authenticationType, pat));

            // arrange
            var firstBuild = JsonConvert.DeserializeObject<Build>(jsonBodyObj.ToString());
        }

        [TestMethod]
        public void GetProjects()
        {
            // arrange
            var httpRequester = new HttpRequester();
            var authenticationType = "Basic";
            var organizationUri = "https://dev.azure.com/MvpProjects";
            var pat = "fyo2u7cy4qypqmoxegg2mlzon6bbf3ykti6ks7dtqsxpmhzg56fa";
            var urlRequestPart = "/_apis/projects";

            // act
            var jsonBodyObj = httpRequester.InvokeGet(
                organizationUri,
                default,
                urlRequestPart,
                default,
                (authenticationType, pat));

            var gg2 = jsonBodyObj.ToString();

            // arrange
            var firstBuild = JsonConvert.DeserializeObject<Projects>(jsonBodyObj.ToString());
        }
    }
}