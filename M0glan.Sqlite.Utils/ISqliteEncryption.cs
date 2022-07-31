namespace M0glan.Sqlite.Utils;

public interface ISqliteEncryption
{
    void SetPassword(string dataSource, string currentPassword = "", string newPassword = "");

    Task SetPasswordAsync(string dataSource, string currentPassword = "", string newPassword = "", CancellationToken cancellationToken = default);

    bool TestPassword(string dataSource, string password);

    Task<bool> TestPasswordAsync(string dataSource, string password, CancellationToken cancellationToken = default);
}