namespace M0glan.Sqlite.Encryption.Test;

using Microsoft.Data.Sqlite;

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
}

public static class SqliteConnectionStringAssert
{       
    public static void OpensConnection(string? dataSource, string? password = "")
    {
        string failMessage = "The dataSource/password combination failed to open the database.";

        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Password = password,
            Mode = SqliteOpenMode.ReadOnly
        }.ConnectionString;
        using var connection = new SqliteConnection(connectionString);

        try
        {
            connection.Open();
            Assert.True(connection.Test(), failMessage);
        }
        catch (SqliteException e)
        {
            Assert.False(e.SqliteErrorCode == SqliteErrorCodes.NOT_A_DB, failMessage);
        }
    }

    public static void DoesNotOpenConnection(string? dataSource, string? password = "")
    {
        string failMessage = "The dataSource/password opened the connection but should have failed.";

        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Password = password,
            Mode = SqliteOpenMode.ReadOnly
        }.ConnectionString;
        using var connection = new SqliteConnection(connectionString);

        try
        {
            connection.Open();
            Assert.False(connection.Test(), failMessage);
        }
        catch (SqliteException e)
        {
            Assert.True(e.SqliteErrorCode == SqliteErrorCodes.NOT_A_DB, failMessage);
        }
    }
}