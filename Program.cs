using Dapper;
using MySqlConnector;
using PhonyPay;

var builder = WebApplication.CreateBuilder(args);
var server = Helpers.GetEnvOrThrow("DB_SERVER");
var username = Helpers.GetEnvOrThrow("DB_USERNAME");
var password = Helpers.GetEnvOrThrow("DB_PASSWORD");
var database = Helpers.GetEnvOrThrow("DATABASE");
var connString = $"Server={server};User ID={username};Password={password};Database={database};";

builder.Services.AddMySqlDataSource(connString);
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Check if the Database is queryable
    var db = scope.ServiceProvider.GetRequiredService<MySqlConnection>();
    db.QuerySingle<int>("SELECT 1");
}

app.MapGet("/api/status", (MySqlConnection db) =>
{
    var res = db.QuerySingle<int>("SELECT 1");
    return new
    {
        status = "OK",
        res
    };
});

app.Run();
