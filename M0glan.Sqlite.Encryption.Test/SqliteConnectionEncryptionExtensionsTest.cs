namespace M0glan.Sqlite.Encryption.Test;

using Microsoft.Data.Sqlite;

public class SqliteConnectionEncryptionExtensionsTest
{
    [Fact]
    public void SetPassword_Should_EncryptDatabase_When_DatabaseIsEmpty()
    {
        string password = "Secret123";

        string? dataSource = TestUtils.GenerateTestFileName(string.Empty, "db3");
        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Mode = SqliteOpenMode.ReadWriteCreate 
        }.ConnectionString;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            connection.SetPassword(password);
        }

        SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource);
        SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "xyz123");
        SqliteConnectionStringAssert.OpensConnection(dataSource, password);
    }

    [Fact]
    public void SetPassword_Should_NotEncryptDatabase_When_DatabaseIsNotEmpty()
    {
        string password = "Secret123";

        string? dataSource = TestUtils.GenerateTestFileName(string.Empty, "db3");
        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Mode = SqliteOpenMode.ReadWriteCreate 
        }.ConnectionString;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            connection.Write();
            connection.SetPassword(password);
        }

        SqliteConnectionStringAssert.OpensConnection(dataSource);
    }

    [Fact]
    public async Task SetPasswordAsync_Should_EncryptDatabase_When_DatabaseIsEmpty()
    {
        string password = "Secret123";

        string? dataSource = TestUtils.GenerateTestFileName(string.Empty, "db3");
        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Mode = SqliteOpenMode.ReadWriteCreate 
        }.ConnectionString;

        using (var connection = new SqliteConnection(connectionString))
        {
            await connection.OpenAsync();
            await connection.SetPasswordAsync(password);
        }

        SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource);
        SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "xyz123");
        SqliteConnectionStringAssert.OpensConnection(dataSource, password);
    }

    [Fact]
    public async Task SetPasswordAsync_Should_NotEncryptDatabase_When_DatabaseIsNotEmpty()
    {
        string password = "Secret123";

        string? dataSource = TestUtils.GenerateTestFileName(string.Empty, "db3");
        string connectionString = new SqliteConnectionStringBuilder() 
        { 
            DataSource = dataSource, 
            Mode = SqliteOpenMode.ReadWriteCreate 
        }.ConnectionString;

        using (var connection = new SqliteConnection(connectionString))
        {
            await connection.OpenAsync();
            connection.Write();
            await connection.SetPasswordAsync(password);
        }

        SqliteConnectionStringAssert.OpensConnection(dataSource);
    }
}