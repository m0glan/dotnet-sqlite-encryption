namespace M0glan.Sqlite.Utils.Test;

public static class TestUtils
{
    public static string GenerateTestFileName(string root, string extension)
    {
        return $"{root}-{DateTimeOffset.UtcNow:yyyy-dd-M--HH-mm-ss}.{extension}";
    }
}