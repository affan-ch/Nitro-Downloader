using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Windows.Storage;

namespace Nitro_Downloader.DBM;

internal class DatabaseHelper
{

    private static readonly string _dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "NitroDownloader.db");

    private static readonly string _connectionString = $"Data Source={_dbPath};Version=3;";


    private static void CreateDatabase()
    {
    
           using var connection = new SQLiteConnection(_connectionString);
           connection.Open();
           //using var command = new SQLiteCommand(_createTableQuery, connection);
           //command.ExecuteNonQuery();
            connection.Close();
    }


}
