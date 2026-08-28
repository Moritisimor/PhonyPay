namespace PhonyPay.Models.Transactions;

public class Transaction
{
    public required int TransactionId { get; set; }
    public required int PayerId { get; set; }
    public required int ReceiverId { get; set; }
    public required int Amount { get; set; }
}
