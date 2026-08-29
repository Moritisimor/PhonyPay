using PhonyPay.Exceptions;
using PhonyPay.Models.Transactions;
using PhonyPay.Repo;

namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static async Task<IResult> GetTransactions(TransactionRepo transactions) => 
        Results.Ok(await transactions.GetTransactions());
    
    public static async Task<IResult> GetTransactionById(TransactionRepo transactions, int id) => 
        Results.Ok(await transactions.GetTransactionById(id));

    public static async Task<IResult> PostTransaction(
        TransactionRepo transactions, 
        TransactionPost transaction,
        AccountRepo accounts)
    {
        try
        {
            var newTransactionId = await transactions.Insert(transaction, accounts);
            return Results.Ok(new { id = newTransactionId });
        }
        catch (NoSuchPayerException e)
        {
            return Results.BadRequest(new { error = e.Message });
        }
        catch (NoSuchReceiverException e)
        {
            return Results.BadRequest(new { error = e.Message });
        }
        catch (PayerIsReceiverException e)
        {
            return Results.BadRequest(new { error = e.Message });
        }
    }
}