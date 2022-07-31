namespace M0glan.Sqlite.Utils;

using Microsoft.Data.Sqlite;

public class SqlCipherEncryption : ISqliteEncryption
{
    public void SetPassword(string dataSource, string currentPassword = "", string newPassword = "")
    {
        throw new NotImplementedException();
    }

    public async Task SetPasswordAsync(string dataSource, string currentPassword = "", string newPassword = "", CancellationToken cancellationToken = default)
    {
        await Task.FromException(new NotImplementedException());
    }

    public bool TestPassword(string dataSource, string password = "")
    {
        using var connection = new SqliteConnection(BuildConnectionString(dataSource, password));
        connection.Open();
        return connection.Test();
    }

    public async Task<bool> TestPasswordAsync(string dataSource, string password = "", CancellationToken cancellationToken = default)
    {
        using var connection = new SqliteConnection(BuildConnectionString(dataSource, password));
        await connection.OpenAsync();
        return await connection.TestAsync();
    }

    private static string BuildConnectionString(string dataSource, string password = "", SqliteOpenMode mode = SqliteOpenMode.ReadOnly)
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = dataSource,
            Password = password,
            Mode = mode
        };
        return builder.ConnectionString;
    }
}
