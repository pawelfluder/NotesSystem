using Microsoft.Data.Sqlite;

namespace SharpSQLiteProj
{
    public class SQLiteService
    {
        public List<string[]> ReadFile(
            string filePath,
            List<int> columnNumbers,
            string sqlQuery)
        {
            var sqlite_conn = TryConnect(filePath);
            var result = ReadData(sqlite_conn, columnNumbers, sqlQuery);
            return result;
            //CreateTable(sqlite_conn);
            //InsertData(sqlite_conn);
        }

        private SqliteConnection TryConnect(string path)
        {
            SqliteConnection sqlite_conn;
            sqlite_conn = CreateConnection(path);
            return sqlite_conn;
        }

        private SqliteConnection CreateConnection(string path)
        {
            SqliteConnection sqlite_conn;
            var success = File.Exists(path);
            sqlite_conn = new SqliteConnection($"Data Source={path}; ");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        private void CreateTable(SqliteConnection conn)
        {

            SqliteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE SampleTable (Col1 VARCHAR(20), Col2 INT)";
            string Createsql1 = "CREATE TABLE SampleTable1 (Col1 VARCHAR(20), Col2 INT)";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = Createsql1;
            sqlite_cmd.ExecuteNonQuery();

        }

        private void InsertData(SqliteConnection conn)
        {
            SqliteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test Text ', 1); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test1 Text1 ', 2); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable (Col1, Col2) VALUES('Test2 Text2 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = "INSERT INTO SampleTable1 (Col1, Col2) VALUES('Test3 Text3 ', 3); ";
            sqlite_cmd.ExecuteNonQuery();
        }

        private List<string[]> ReadData(
            SqliteConnection conn,
            List<int> columnNumbers,
            string sqlQuery)
        {
            SqliteDataReader sqlite_datareader;
            SqliteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = sqlQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            var result = new List<string[]>();
            
            while (sqlite_datareader.Read())
            {
                try
                {
                    var i = 0;
                    var values = new string[columnNumbers.Count];
                    foreach (var num in columnNumbers)
                    {
                        values[i] = sqlite_datareader.GetString(num);
                        i++;
                    }

                    result.Add(values);
                }
                catch (Exception ex)
                {
                }
            }

            conn.Close();
            return result;
        }
    }
}

