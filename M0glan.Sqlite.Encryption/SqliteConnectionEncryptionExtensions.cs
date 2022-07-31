namespace M0glan.Sqlite.Encryption;

using Microsoft.Data.Sqlite;

public static class SqliteConnectionEncryptionExtensions
{
    private const string TEST_SQL = "SELECT count(*) FROM sqlite_master";

    /// <summary>
    /// Sets the encryption key for a SQLite database to the value of the password parameter. 
    /// This only works to set the password on a fresh database (that has never been written to).
    /// </summary>
    /// <param name="connection">The target connection.</param>
    /// <param name="password">The encryption key.</param>
    public static void SetPassword(this SqliteConnection connection, string password)
    {
        string quotedPassword = connection.GetQuotedParameter(password);
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"PRAGMA key = {quotedPassword}";
        command.ExecuteNonQuery();

        command.CommandText = $"PRAGMA rekey = {quotedPassword}";
        command.ExecuteNonQuery();
    }

    private static string GetQuotedParameter(this SqliteConnection connection, string @param)
    {
        using SqliteCommand command = connection.CreateQuoteCommand(@param);
        object? commandResult = command.ExecuteScalar();
        return commandResult is string quotedParameter ? quotedParameter : string.Empty;
    }

    /// <summary>
    /// Sets the encryption key for a SQLite database to the value of the password parameter. 
    /// This only works to set the password on a fresh database (that has never been written to).
    /// </summary>
    /// <param name="connection">The target connection.</param>
    /// <param name="password">The encryption key.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    public static async Task SetPasswordAsync(this SqliteConnection connection, string password, CancellationToken cancellationToken = default)
    {
        string quotedPassword = await connection.GetQuotedParameterAsync(password, cancellationToken);
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"PRAGMA key = {quotedPassword}";
        await command.ExecuteNonQueryAsync(cancellationToken);

        command.CommandText = $"PRAGMA rekey = {quotedPassword}";
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task<string> GetQuotedParameterAsync(this SqliteConnection connection, string @param, CancellationToken cancellationToken)
    {
        using SqliteCommand command = connection.CreateQuoteCommand(@param);
        object? commandResult = await command.ExecuteScalarAsync(cancellationToken);
        return commandResult is string quotedParameter ? quotedParameter : string.Empty;
    }

    private static SqliteCommand CreateQuoteCommand(this SqliteConnection connection, string @param)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT quote(@parameter)";
        command.Parameters.AddWithValue("@parameter", @param);
        return command;
    }

    public static bool Test(this SqliteConnection connection)
    {
        try 
        {
            using SqliteCommand command = connection.CreateTestCommand();
            command.ExecuteScalar();
            return true;
        }
        catch (SqliteException e)
        {
            return !FailedToOpenDatabaseOrThrow(e);
        }
    }

    public static async Task<bool> TestAsync(this SqliteConnection connection)
    {
        try 
        {
            using SqliteCommand command = connection.CreateTestCommand();
            await command.ExecuteScalarAsync();
            return true;
        }
        catch (SqliteException e)
        {
            return !FailedToOpenDatabaseOrThrow(e);
        }
    }

    private static SqliteCommand CreateTestCommand(this SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = TEST_SQL;
        return command;
    }

    private static bool FailedToOpenDatabaseOrThrow(SqliteException e)
    {
        if (FailedToOpenDatabase(e))
        {
            return true;
        }

        throw e;
    }

    private static bool FailedToOpenDatabase(SqliteException e)
    {
        return e.SqliteErrorCode == SqliteErrorCodes.NOT_A_DB;
    }
}