using PhonyPay.Repo;

namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static IResult GetTransactions(TransactionRepo transactions) => 
        Results.Ok(transactions.GetTransactions().Result);
}