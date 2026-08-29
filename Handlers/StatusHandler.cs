using Dapper;
using MySqlConnector;

namespace PhonyPay.Handlers;

public static partial class Handlers
{
    public static IResult Status() => Results.Ok(new { status = "OK" });

    public static IResult DatabaseStatus(MySqlConnection conn)
    {
        conn.QuerySingleAsync("SELECT 1");
        return Results.Ok(new { status = "OK" });
    }
}
