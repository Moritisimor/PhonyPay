namespace PhonyPay.Models.Transactions;

public class Transaction
{
    public required int TransactionId { get; set; }
    public required int SenderId { get; set; }
    public required int ReceiverId { get; set; }
    public required double Amount { get; set; }
}
