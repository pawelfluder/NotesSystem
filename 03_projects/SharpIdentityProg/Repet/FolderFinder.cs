using System.Text.RegularExpressions;

namespace SharpIdentityProg.Repet
{
    public class FolderFinder
    {
        public string FindFolder(
            string searchFolderName,
            string inputFolderPath,
            string expression)
        {
            var success = FindRange(expression, out int move,
                out (int left, int right) range);

            var s1 = MoveDirectoriesUp(inputFolderPath, -move, out var inputFolderPath2);

            var gg1 = FindFolderInRangeDown(searchFolderName, inputFolderPath2, range.right + 1);
            if (gg1 != default)
            {
                return gg1;
            }

            var gg2 = FindFolderInRangeUp(searchFolderName, inputFolderPath2, -range.left + 1);

            if (gg2 != default)
            {
                return gg2;
            }

            return "";
        }

        public bool MoveDirectoriesUp(
            string inputFolderPath,
            int level,
            out string outputFolderPath)
        {
            outputFolderPath = inputFolderPath;

            for (int i = 0; i < level; i++)
            {
                try
                {
                    var tmp = Directory.GetParent(outputFolderPath);
                    outputFolderPath = tmp.FullName;
                }
                catch
                {
                    outputFolderPath = default;
                    return false;
                }
            }

            return true;
        }

        public bool FindRange(
            string expression,
            out int move,
            out (int left, int right) range)
        {
            move = default;
            range = default;

            if (string.IsNullOrEmpty(expression))
            {
                return false;
            }

            var regex01 = "^(-?[0-9]\\d*)\\((-?[0-9]\\d*),(-?[0-9]\\d*)\\)$";
            var match01 = Regex.Match(expression, regex01);
            if (match01.Groups.Count == 3 + 1)
            {
                var success01 = int.TryParse(
                    match01.Groups.Values.ElementAt(1).Value,
                    out var tmp01);

                var success02 = int.TryParse(
                    match01.Groups.Values.ElementAt(2).Value,
                    out var tmp02);

                var success03 = int.TryParse(
                    match01.Groups.Values.ElementAt(3).Value,
                    out var tmp03);

                if (success01 && success02 && success03)
                {
                    move = tmp01;
                    range.left = tmp02;
                    range.right = tmp03;
                }

                return true;
            }

            var regex02 = "^\\((-?[0-9]\\d*),(-?[0-9]\\d*)\\)$";
            var match02 = Regex.Match(expression, regex02);
            if (match02.Groups.Count == 2 + 1)
            {
                var success01 = int.TryParse(
                    match02.Groups.Values.ElementAt(1).Value,
                    out var tmp01);

                var success02 = int.TryParse(
                    match02.Groups.Values.ElementAt(2).Value,
                    out var tmp02);

                if (success01 && success02)
                {
                    move = 0;
                    range.left = tmp01;
                    range.right = tmp02;
                }

                return true;
            }

            var regex03 = "^\\((-?[0-9]\\d*)\\)$";
            var match03 = Regex.Match(expression, regex03);
            if (match03.Groups.Count == 1 + 1)
            {
                var success01 = int.TryParse(
                    match03.Groups.Values.ElementAt(1).Value,
                    out var tmp01);

                if (success01)
                {
                    move = 0;
                    range.left = tmp01;
                    range.right = 0;
                }

                return true;
            }

            return false;
        }

        public string FindFolderInRangeUp(
            string folderName,
            string folderPath,
            int max)
        {
            string startupProjectFolder = default;
            string[] directories = null;
            var currentFolder = folderPath;

            for (var i = 0; i <= max; i++)
            {
                if (currentFolder == null)
                {
                    return default;
                }

                directories = Directory.GetDirectories(currentFolder);
                startupProjectFolder = directories.SingleOrDefault(
                    x => Path.GetFileName(x) == folderName);

                if (startupProjectFolder != default)
                {
                    return startupProjectFolder;
                }

                var success = MoveDirectoriesUp(currentFolder, 1, out var outputFolderPath);

                if (!success)
                {
                    return default;
                }

                currentFolder = outputFolderPath;
            }

            return default;
        }

        public string FindFolderInRangeDown(
            string folderName,
            string folderPath,
            int max)
        {
            string foundFolder = default;
            string[] directories = null;

            if (max > 0)
            {
                directories = Directory.GetDirectories(folderPath);
                foundFolder = directories.SingleOrDefault(
                    x => Path.GetFileName(x) == folderName);

                if (foundFolder != default)
                {
                    return foundFolder;
                }

                foreach (var dir in directories)
                {
                    if (!IsSpecial(dir))
                    {
                        var found = FindFolderInRangeDown(folderName, dir, max - 1);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }

            return default;
        }

        public bool IsSpecial(string folderPath)
        {
            if (folderPath == "Config.Msi" ||
                folderPath == "$RECYCLE.BIN")
            {
                return true;
            }

            return false;
        }
    }
}
