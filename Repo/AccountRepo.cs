using Dapper;
using MySqlConnector;
using PhonyPay.Models.Accounts;

namespace PhonyPay.Repo;

public class AccountRepo(MySqlConnection conn)
{
    public async Task Migrate() =>
        await conn.ExecuteAsync("""
                          CREATE TABLE IF NOT EXISTS Accounts (
                              AccountId INTEGER PRIMARY KEY AUTO_INCREMENT,
                              FirstName VARCHAR(50) NOT NULL,
                              LastName VARCHAR(50) NOT NULL,
                              Balance REAL NOT NULL
                          )
                          """);
    
    public async Task Insert(AccountPost account)
    {
        await conn.ExecuteAsync(
            "INSERT INTO Accounts (FirstName, LastName, Balance) VALUES (@FirstName, @LastName, @Balance)",
            new
            {
                account.FirstName,
                account.LastName,
                Balance = 0.0
            });
    }

    public async Task WithdrawFromAccountWithId(int accountId, int amount) =>
        await conn.ExecuteAsync(
            "UPDATE Accounts SET Balance = Balance - @amount WHERE AccountID = @accountId", 
            new { accountId, amount });

    public async Task<Account> GetAccountById(int accountId) =>
        await conn.QuerySingleAsync<Account>(
            "SELECT * FROM Accounts WHERE AccountID = @accountId", 
            new { accountId });
 
    public async Task<IEnumerable<Account>> GetAccounts() =>
        await conn.QueryAsync<Account>("SELECT * FROM Accounts");

    public async Task DepositToAccountWithId(int accountId, int amount) =>
        await conn.ExecuteAsync(
            "UPDATE Accounts SET Balance = Balance + @amount WHERE AccountID = @accountId", 
            new { accountId, amount });
    
    public async Task DeleteAccountWithId(int accountId) =>
        await conn.ExecuteAsync("DELETE FROM Accounts WHERE AccountID = @accountId", new { accountId });
}