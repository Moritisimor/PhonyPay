using Dapper;
using MySqlConnector;
using PhonyPay.Models.Transactions;

namespace PhonyPay.Repo;

public class TransactionRepo(MySqlConnection conn)
{
    public async Task Migrate() =>
        await conn.ExecuteAsync("""
                                CREATE TABLE IF NOT EXISTS Transactions (
                                    TransactionId INTEGER PRIMARY KEY AUTO_INCREMENT,
                                    PayerId INTEGER NOT NULL,
                                    ReceiverId INTEGER NOT NULL,
                                    Amount REAL NOT NULL
                                )
                                """);

    public async Task Insert(TransactionPost transaction) =>
        await conn.ExecuteAsync(
            "INSERT INTO Transactions (PayerId, ReceiverId, Amount) VALUES (@PayerId, @ReceiverId, @Amount)", 
            transaction);
    
    public async Task<IEnumerable<Transaction>> GetTransactions() =>
        await conn.QueryAsync<Transaction>("SELECT * FROM Transactions");
    
    public async Task<Transaction> GetTransactionById(int transactionId) =>
        await conn.QuerySingleAsync<Transaction>(
            "SELECT * FROM Transactions WHERE TransactionId = @transactionId", 
            new { transactionId });
    
    public async Task DeleteTransactionWithId(int transactionId) =>
        await conn.ExecuteAsync("DELETE FROM Transactions WHERE TransactionId = @transactionId", new { transactionId });
}