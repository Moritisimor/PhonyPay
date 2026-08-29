namespace PhonyPay.Exceptions;

public class InsufficientBalanceException(string message) : Exception(message);