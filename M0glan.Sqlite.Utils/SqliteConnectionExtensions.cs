namespace M0glan.Sqlite.Utils;

using Microsoft.Data.Sqlite;

public static class SqliteConnectionExtensions
{
    public static void SetPassword(this SqliteConnection connection, string password)
    {
        string quotedPassword = connection.GetQuotedParameter(password);

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"PRAGMA key = {quotedPassword}";
        command.ExecuteNonQuery();
    }

    public static string GetQuotedParameter(this SqliteConnection connection, string @param)
    {
        using SqliteCommand command = connection.CreateQuoteCommand(@param);
        object? commandResult = command.ExecuteScalar();

        if (commandResult is string quotedParameter)
        {
            return quotedParameter;
        }

        return string.Empty;
    }

    public static async Task SetPasswordAsync(this SqliteConnection connection, string password)
    {
        string quotedPassword = await connection.GetQuotedParameterAsync(password);

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"PRAGMA key = {quotedPassword}";
        await command.ExecuteNonQueryAsync();
    }

    public static async Task<string> GetQuotedParameterAsync(this SqliteConnection connection, string @param)
    {
        using SqliteCommand command = connection.CreateQuoteCommand(@param);
        object? commandResult = await command.ExecuteScalarAsync();

        if (commandResult is string quotedParameter)
        {
            return quotedParameter;
        }

        return string.Empty;
    }

    private static SqliteCommand CreateQuoteCommand(this SqliteConnection connection, string @param)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT quote(@parameter)";
        command.Parameters.AddWithValue("@parameter", @param);
        return command;
    }
}