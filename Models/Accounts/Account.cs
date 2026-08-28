namespace PhonyPay.Models.Accounts;

public class Account
{
    public required int AccountId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required double Balance { get; set; }
}