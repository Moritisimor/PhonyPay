namespace PhonyPay;

public class Helpers
{
    public static string GetEnvOrThrow(string envVar) =>
        Environment.GetEnvironmentVariable(envVar) switch
        {
            { } s => s,
            null => throw new InvalidOperationException($"Environment variable {envVar} not found. Set it!")
        };
}