using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Gaver.TestUtils;

public static class DbUtils
{
    public static DbConnection CreateInMemoryDbConnection()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }
}
