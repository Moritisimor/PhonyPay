using PhonyPay.Repo;

namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static IResult GetAccounts(AccountRepo accounts) => Results.Ok(accounts.GetAccounts().Result);
    
}