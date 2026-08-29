namespace PhonyPay.Exceptions;

public class ZeroOrNegativeAmountException(string message) : Exception(message);
