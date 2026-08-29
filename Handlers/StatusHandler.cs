namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static IResult Status() => Results.Ok(new { status = "OK" });
}
