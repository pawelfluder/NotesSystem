using Microsoft.Extensions.Configuration;

namespace SharpIdentityProg.Repet
{
    public class ConnectionStringHelper
    {
        private readonly FolderFinder folderFinder;
        string repetPart;

        public ConnectionStringHelper()
        {
            folderFinder = new FolderFinder();

            var databaseName = "ReactAspnetExample";
            repetPart =
                $"Database={databaseName};" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=true;";
        }

        public string Get(IConfiguration configuration)
        {
            var isAppConfigOurChoice = false;
            var result = string.Empty;

            string GetName()
            {
                return ConnectionTypes.DebugSqlLite;
                var machineName = Environment.MachineName;
                if (machineName == "DESKTOP-D1H8AMA") { return ConnectionTypes.DesktopComputer; }
                if (machineName == "DESKTOP-FS2O85U") { return ConnectionTypes.Surface; }
                return default;
            }

            var name = GetName();

            if (isAppConfigOurChoice)
            {
                result = configuration
                    .GetConnectionString(name);
                return result;
            }

            if (!isAppConfigOurChoice)
            {
                result = Get(name);
                return result;
            }

            return result;
        }

        public string Get()
        {
            return Get(ConnectionTypes.Surface);
        }

        private string Get(string type)
        {
            if (type == ConnectionTypes.DesktopComputer)
            {
                var result = GetForDesktopComputer();
                return result;
            }
            if (type == ConnectionTypes.Surface)
            {
                var result = GetForLaptop();
                return result;
            }
            if (type == ConnectionTypes.DebugSqlLite)
            {
                var result = GetSqLite();
                return result;
            }
            if (type == ConnectionTypes.ReleaseSqlLite)
            {
                return "";
            }

            return "";
        }

        public string GetSqLite()
        {
            var connectionString = "Data Source=" + GetSqLiteFilePath();
            return connectionString;
        }

        public string GetForLaptop()
        {
            return "Server=DESKTOP-FS2O85U;" + repetPart;
        }

        public string GetForDesktopComputer()
        {
            return "Server=DESKTOP-D1H8AMA;" + repetPart;
        }

        public string GetSqLiteFilePath()
        {
            var folderName = "SQLiteDatabase";
            var dbFileName = "MyCoolDataBase.db";
            var dbFolderPath = folderFinder.FindFolder(folderName, Environment.CurrentDirectory, "-4(2,4)");
            dbFolderPath = dbFolderPath.Replace('\\', '/');
            var dbFilePath = dbFolderPath + "/" + dbFileName;
            return dbFilePath;
        }
    }
}
