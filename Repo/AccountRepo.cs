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
    
    public async Task<int> Insert(AccountPost account) =>
        await conn.QuerySingleAsync<int>(
            """
            INSERT INTO Accounts (FirstName, LastName, Balance) VALUES (@FirstName, @LastName, @Balance);
            SELECT LAST_INSERT_ID();
            """,
            new
            {
                account.FirstName,
                account.LastName,
                Balance = 0.0
            });
    

    public async Task<double> WithdrawFromAccountWithId(int accountId, double amount) =>
        await conn.QuerySingleAsync<double>(
            """
            UPDATE Accounts SET Balance = Balance - @amount WHERE AccountID = @accountId;
            SELECT Balance FROM Accounts WHERE AccountID = @accountId;
            """, 
            new { accountId, amount });

    public async Task<Account?> GetAccountById(int accountId) =>
        await conn.QuerySingleOrDefaultAsync<Account?>(
            "SELECT * FROM Accounts WHERE AccountID = @accountId", 
            new { accountId });
 
    public async Task<IEnumerable<Account>> GetAccounts() =>
        await conn.QueryAsync<Account>("SELECT * FROM Accounts");

    public async Task<double> DepositToAccountWithId(int accountId, double amount) =>
        await conn.QuerySingleAsync<double>(
            """
            UPDATE Accounts SET Balance = Balance + @amount WHERE AccountID = @accountId;
            SELECT Balance FROM Accounts WHERE AccountID = @accountId;
            """, 
            new { accountId, amount });
    
    public async Task DeleteAccountWithId(int accountId) =>
        await conn.ExecuteAsync("DELETE FROM Accounts WHERE AccountID = @accountId", new { accountId });
}
