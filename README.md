# .NET SQLite Encryption
This library aims to provide a set of `Microsoft.Data.Sqlite.SqliteConnection` extension methods and other SQLite database encryption utilities. The tools are built around [SQLCipher](https://www.zetetic.net/sqlcipher/about/).

## Examples

```csharp
using Microsoft.Data.Sqlite;
using M0glan.Sqlite.Encryption;

var connectionString = new SqliteConnectionStringBuilder
{
    DataSource = @"/some/directory/test.db",
    Mode = SqliteOpenMode.ReadWriteCreate
}.ConnectionString;

using var connection = new SqliteConnection(connectionString);
connection.Open();
connection.SetPassword("Secret123!");
```

**Note:** It is important for the database to not yet exist. The underlying SQLCipher technology cannot encrypt an existing/already populated database.