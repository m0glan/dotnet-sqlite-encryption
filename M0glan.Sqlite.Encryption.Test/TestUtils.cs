using Microsoft.Data.Sqlite;

namespace M0glan.Sqlite.Encryption.Test;

public static class TestUtils
{
    public static string? GenerateTestFileName(string root, string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentNullException(nameof(extension));
        }

        string generated = $"{Guid.NewGuid()}-{DateTimeOffset.UtcNow:yyyy-dd-M--HH-mm-ss}";

        if (string.IsNullOrWhiteSpace(root))
        {
            return Path.ChangeExtension(generated, extension);
        }

        return Path.ChangeExtension($"{root}-{generated}", extension);
    }
}

public static class SqliteConnectionTestExtensions
{
    public static void Write(this SqliteConnection connection)
    {
        using SqliteCommand createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText = "CREATE TABLE TestTable (Id INT PRIMARY KEY)";
        createTableCommand.ExecuteNonQuery();

        using SqliteCommand insertRecordCommand = connection.CreateCommand();
        insertRecordCommand.CommandText = "INSERT INTO TestTable VALUES (1)";
        insertRecordCommand.ExecuteNonQuery();
    }

    public static async Task WriteAsync(this SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE TestTable (Id INT PRIMARY KEY)";
        await command.ExecuteNonQueryAsync();
    }
}