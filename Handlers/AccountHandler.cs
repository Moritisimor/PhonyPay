using PhonyPay.Models.Accounts;
using PhonyPay.Repo;

namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static async Task<IResult> GetAccounts(AccountRepo accounts) => Results.Ok(await accounts.GetAccounts());
    public static async Task<IResult> GetAccountById(AccountRepo accounts, int id) =>
        await accounts.GetAccountById(id) switch
        {
            {} a => Results.Ok(a),
            null => Results.NotFound(new { error = "Account not found" })
        };

    public static async Task<IResult> WithdrawFromAccount(
        AccountRepo accountRepo, 
        AccountBalanceChangePost? withdrawData)
    {
        if (withdrawData is null) 
            return Results.BadRequest();

        try
        {
            var newBalance = await accountRepo.WithdrawFromAccountWithId( 
                withdrawData.AccountId, 
                withdrawData.Amount);
            
            return Results.Ok(new { newBalance });
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound(new { error = "Account not found" });
        }
    }

    public static async Task<IResult> DepositToAccount(AccountRepo accountRepo, AccountBalanceChangePost? depositData)
    {
        if (depositData is null)
            return Results.BadRequest();

        try
        {
            var newBalance = await accountRepo.DepositToAccountWithId(depositData.AccountId, depositData.Amount);
            return Results.Ok(new { newBalance });   
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound(new { error = "Account not found" });
        }
    }

    public static async Task<IResult> RegisterAccount(AccountRepo accountRepo, AccountPost? accountPost)
    {
        if (accountPost is null) 
            return Results.BadRequest();
        
        var insertId = await accountRepo.Insert(accountPost);
        return Results.Ok(new { id = insertId });
    }
}