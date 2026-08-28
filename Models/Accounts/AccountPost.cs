using System.ComponentModel.DataAnnotations;

namespace PhonyPay.Models.Accounts;

public class AccountPost
{
    [Required]
    public required string FirstName { get; set; }
    
    [Required]
    public required string LastName { get; set; }
}