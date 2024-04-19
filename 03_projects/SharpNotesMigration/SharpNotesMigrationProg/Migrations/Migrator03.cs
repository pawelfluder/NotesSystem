using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using System.Text.RegularExpressions;

namespace SharpNotesMigrationProg.Migrations
{
    internal class Migrator03 : IMigrator, IMigrator03
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;
        private readonly IFileService.IYamlOperations yamlOperations;
        private string pattern1;
        private string pattern2;
        private string pattern3;
        private string pattern4;
        private string keyName;
        private bool agree;

        public List<(int, string, string, string)> Changes { get; private set; }

        public Migrator03(
            IFileService fileService,
            IRepoService repoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
            yamlOperations = fileService.Yaml.Custom03;
            Changes = new List<(int, string, string, string)>();

            SetPatterns();
        }

        private void SetPatterns()
        {
            keyName = "name";
            var str1 = "{0}:";
            var str2 = "\"{0}\":";
            var str3 = "'{0}':";
            var str4 = "\\\"{0}\\\":";

            pattern1 = string.Format(str1, keyName);
            pattern2 = string.Format(str2, keyName);
            pattern3 = string.Format(str3, keyName);
            pattern4 = string.Format(str4, keyName);
        }

        public void MigrateEverything()
        {
            var allRepoNameList = repoService.Methods.GetAllReposNames();
            foreach (var repoName in allRepoNameList)
            {
                MigrateOneRepo((repoName, ""));
            }
        }

        public void MigrateOneRepo((string Repo, string Loca) address)
        {
            var foundAddressList = repoService.Methods.GetAllRepoAddresses(address);
            foreach (var foundAddress in foundAddressList)
            {
                MigrateOneAddress(foundAddress);
            }
        }

        private void ClearNameFile((string Repo, string Loca) address)
        {
            var path = repoService.Methods.GetConfigPath(address);
            var exists = File.Exists(path);
            if (!exists)
            {
                using (File.Create(path))
                {}
            }
        }

        public void MigrateOneAddress((string Repo, string Loca) address)
        {
            ClearNameFile(address);
            if (!IsJustOneLine(address, out var nameLine))
            {
                //HandleError();
                return;
            }

            var before = nameLine;
            var clear = "nothing";
            var after = "nothing";
            var changeType = 0;

            var isAlreadyMigrated = IsAlreadyMigrated(nameLine);
            var hasDuplicates = HasDuplicates(nameLine);

            if (isAlreadyMigrated &&
                hasDuplicates)
            {
                before = nameLine;
                nameLine = ClearFromDuplicates3(nameLine);
                clear = nameLine;
                after = MigrateStringToYamlDict(address, nameLine);
                changeType = 2;
            }

            if (!isAlreadyMigrated)
            {
                before = nameLine;
                after = MigrateStringToYamlDict(address, nameLine);
                changeType = 1;
            }

            Changes.Add((changeType, before, clear, after));
        }

        private bool IsJustOneLine((string, string) address, out string line)
        {
            var path = repoService.Methods.GetConfigPath(address);
            var lineList = File.ReadAllLines(path);
            //var success = repoService.Methods.TryGetConfigLines(address, out var lines);
            if (lineList.Count() == 1)
            {
                line = lineList.First();
                return true;
            }

            line = null;
            return false;
        }

        private bool HasDuplicatesOld(string nameLine)
        {
            var match1 = Regex.Match(nameLine, pattern1);
            var match2 = Regex.Match(nameLine, pattern2);
            var match3 = Regex.Match(nameLine, pattern3);
            //var match4 = Regex.Match(nameLine, pattern4);

            var count = match1.Captures.Count +
                match2.Captures.Count +
                match3.Captures.Count;
            //match4.Captures.Count;

            if (count >= 2)
            {
                return true;
            }

            return false;
        }

        private bool HasDuplicates(string nameLine)
        {
            var n = 0;
            var tmp = nameLine;
            while (true)
            {
                try
                {
                    var dict = yamlOperations.Deserialize<Dictionary<string, object>>(tmp);
                    tmp = dict["name"].ToString();
                    n++;
                }
                catch
                {
                    break;
                }
            }

            if (n >= 2)
            {
                return true;
            }

            return false;
        }

        public string ClearFromDuplicates3(string nameLine)
        {
            var success = true;

            while (success)
            {
                success = yamlOperations.TryDeserialize<Dictionary<string, object>>
                    (nameLine, out var dict);
                if (dict == null) { break; }
                if (!success) { break; }
                nameLine = dict.Values.First().ToString();
            }

            return nameLine;
        }

        public string ClearFromDuplicates2(string nameLine)
        {
            //^[a-zA-Z0-9_.-]*$
            var regex1 = $"^{pattern1} \"([\\t-ý]*)\"$";
            var match1 = Regex.Match(nameLine, regex1);
            if (match1.Groups.Count == 2) { return match1.Groups[1].Value; }

            var regex2 = $"^{pattern2} \"([\\t-ý]*)\"$";
            var match2 = Regex.Match(nameLine, regex2);
            if (match2.Groups.Count == 2) { return match2.Groups[1].Value; }

            var regex3 = $"^{pattern3} \"([\\t-ý]*)\"$";
            var match3 = Regex.Match(nameLine, regex3);
            if (match3.Groups.Count == 2) { return match3.Groups[1].Value; }

            var regex4 = $"^{pattern4} \"([\\t-ý]*)\"$";
            var match4 = Regex.Match(nameLine, regex4);
            if (match4.Groups.Count == 2) { return match4.Groups[1].Value; }

            return "";
        }

        public string ClearFromDuplicates(string nameLine)
        {
            Match match1 = Regex.Match(nameLine, pattern1);
            Match match2 = Regex.Match(nameLine, pattern2);
            Match match3 = Regex.Match(nameLine, pattern3);
            //Match match4 = Regex.Match(nameLine, str4);

            if (match1.Captures.Count > 0)
            {
                nameLine = nameLine.Replace(pattern1, "");
            }

            if (match2.Captures.Count > 0)
            {
                nameLine = nameLine.Replace(pattern2, "");
            }

            if (match3.Captures.Count > 0)
            {
                nameLine = nameLine.Replace(pattern3, "");
            }

            nameLine = nameLine.Replace("\\\"", "");
            nameLine = nameLine.Replace("\\", "");

            return nameLine;
        }

        private bool IsAlreadyMigrated(string nameLine)
        {
            if (nameLine.StartsWith(string.Format(pattern1, keyName)) ||
                nameLine.StartsWith(string.Format(pattern2, keyName)) ||
                nameLine.StartsWith(string.Format(pattern3, keyName)))
            {
                return true;
            }

            return false;
        }

        private string MigrateStringToYamlDict((string Repo, string Loca) address, string nameLine)
        {
            var dict = new Dictionary<string, string> { { "name", nameLine } };
            var newLines = yamlOperations.Serialize(dict).Replace("\r\n", "");
            var outDict = yamlOperations.Deserialize<Dictionary<string, object>>(newLines);
            var correct1 = dict.Keys.ElementAt(0) == outDict.Keys.ElementAt(0);
            var correct2 = dict.Values.ElementAt(0).ToString() == outDict.Values.ElementAt(0).ToString();
            var correct = correct1 && correct2;

            if (!correct)
            {
                throw new Exception();
            }

            if (agree)
            {
                repoService.Methods.CreateConfig(address, new List<string> { newLines });
            }

            return newLines;
        }

        private bool IsPlainFormat(string nameLine)
        {
            var gg1 = !nameLine.Contains(": ");
            var gg2 = nameLine.StartsWith("'");

            return true;
        }

        private void CorrectToDoubleQuoted((string Repo, string Loca) address, string nameLine)
        {
            var outDict = yamlOperations.Deserialize<Dictionary<string, object>>(nameLine);
            var name = outDict["name"].ToString();
            MigrateStringToYamlDict(address, name);
        }

        private bool IsSingleQuoteFormat(string nameLine)
        {
            if (nameLine.StartsWith("name: '") &&
                nameLine.EndsWith("'"))
            {
                return true;
            }

            return false;
        }

        private bool IsDoubleQuoteFormat(string nameLine)
        {
            if (nameLine.StartsWith("name: \"") &&
                nameLine.EndsWith("\""))
            {
                return true;
            }

            return false;
        }

        private void HandleError()
        {
            throw new NotImplementedException();
        }

        public void SetAgree(bool agree)
        {
            this.agree = agree;
        }


        public void MigrateOneFolder((string Repo, string Loca) adrTuple)
        {
            var foundAddressList = repoService.Methods
                .GetAllRepoAddresses(adrTuple).ToList();

            MigrateOneAddress(adrTuple);

            foreach (var foundAddress in foundAddressList)
            {
                MigrateOneAddress(foundAddress);
            }
        }

        public void MigrateOneRepo(string repoName)
        {
            throw new NotImplementedException();
        }

        public void MigrateAllRepos()
        {
            throw new NotImplementedException();
        }
    }
}
