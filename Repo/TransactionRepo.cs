using Dapper;
using MySqlConnector;
using PhonyPay.Exceptions;
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

    public async Task<int> Insert(TransactionPost transaction)
    {
        if (transaction.SenderId == transaction.ReceiverId)
            throw new PayerIsReceiverException("You cannot transfer money to yourself");

        if (transaction.Amount <= 0)
            throw new ZeroOrNegativeAmountException("Cannot transfer a negative or zero amount");
        
        await conn.OpenAsync();
        var accounts = new AccountRepo(conn);
        
        var receiver = await accounts.GetAccountById(transaction.ReceiverId);
        if (receiver is null)
            throw new NoSuchReceiverException($"No account with this ID: {transaction.ReceiverId}");

        var payer = await accounts.GetAccountById(transaction.SenderId);
        if (payer is null)
            throw new NoSuchPayerException($"No account with this ID: {transaction.SenderId}");

        await using var tx = await conn.BeginTransactionAsync();
        try
        {
            await accounts.WithdrawFromAccountWithId(transaction.SenderId, transaction.Amount, tx);
            await accounts.DepositToAccountWithId(transaction.ReceiverId, transaction.Amount, tx);
            var insertId = await conn.QuerySingleAsync<int>(
                """
                INSERT INTO Transactions (PayerId, ReceiverId, Amount) 
                VALUES (@PayerId, @ReceiverId, @Amount);

                SELECT LAST_INSERT_ID();
                """,
                new
                {
                    PayerId = transaction.SenderId,
                    transaction.ReceiverId,
                    transaction.Amount
                },
                transaction: tx);
            
            await tx.CommitAsync();
            return insertId;
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactions() =>
        await conn.QueryAsync<Transaction>("SELECT * FROM Transactions");
    
    public async Task<Transaction> GetTransactionById(int transactionId) =>
        await conn.QuerySingleAsync<Transaction>(
            "SELECT * FROM Transactions WHERE TransactionId = @transactionId", 
            new { transactionId });
    
    public async Task DeleteTransactionWithId(int transactionId) =>
        await conn.ExecuteAsync("DELETE FROM Transactions WHERE TransactionId = @transactionId", new { transactionId });
}