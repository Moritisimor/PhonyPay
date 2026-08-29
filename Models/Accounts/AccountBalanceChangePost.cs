using System.ComponentModel.DataAnnotations;

namespace PhonyPay.Models.Accounts;

public class AccountBalanceChangePost
{
    [Required]
    public required int AccountId { get; set; }
    
    [Required]
    public required double Amount { get; set; }
}
