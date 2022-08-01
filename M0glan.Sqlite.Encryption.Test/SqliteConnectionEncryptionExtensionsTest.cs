using Microsoft.Data.Sqlite;

namespace M0glan.Sqlite.Encryption.Test
{
    public class SqliteConnectionEncryptionExtensionsTest
    {
        [Fact]
        public void SetPassword_Should_EncryptDatabase_When_DatabaseIsFresh()
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

            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "");
            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "xyz123");
            SqliteConnectionStringAssert.OpensConnection(dataSource, password);
        }

        [Fact]
        public void SetPassword_Should_ChangePassword()
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

            string newPassword = "xyz123";

            connectionString = new SqliteConnectionStringBuilder() 
            { 
                DataSource = dataSource,
                Password = password,
                Mode = SqliteOpenMode.ReadWriteCreate 
            }.ConnectionString;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                connection.SetPassword(newPassword);
            }

            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource);
            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, password);
            SqliteConnectionStringAssert.OpensConnection(dataSource, newPassword);
        }

        [Fact]
        public async Task SetPasswordAsync_Should_EncryptDatabase_When_DatabaseIsFresh()
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

            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "");
            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, "xyz123");
            SqliteConnectionStringAssert.OpensConnection(dataSource, password);
        }

        [Fact]
        public async Task SetPasswordAsync_Should_ChangePassword()
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

            string newPassword = "xyz123";

            connectionString = new SqliteConnectionStringBuilder() 
            { 
                DataSource = dataSource,
                Password = password,
                Mode = SqliteOpenMode.ReadWriteCreate 
            }.ConnectionString;
            using (var connection = new SqliteConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.SetPasswordAsync(newPassword);
            }

            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource);
            SqliteConnectionStringAssert.DoesNotOpenConnection(dataSource, password);
            SqliteConnectionStringAssert.OpensConnection(dataSource, newPassword);
        }
    }
}