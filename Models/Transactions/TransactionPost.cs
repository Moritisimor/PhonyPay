using System.ComponentModel.DataAnnotations;

namespace PhonyPay.Models.Transactions;

public class TransactionPost
{
    [Required]
    public required int ReceiverId { get; set; }
    
    [Required]
    public required int SenderId { get; set; }
    
    [Required] 
    public required double Amount { get; set; }
}