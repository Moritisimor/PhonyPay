using MySqlConnector;
using PhonyPay;
using PhonyPay.Handlers;
using PhonyPay.Repo;

var builder = WebApplication.CreateBuilder(args);
var server = Helpers.GetEnvOrThrow("DB_SERVER");
var username = Helpers.GetEnvOrThrow("DB_USERNAME");
var password = Helpers.GetEnvOrThrow("DB_PASSWORD");
var database = Helpers.GetEnvOrThrow("DATABASE");
var connString = $"Server={server};User ID={username};Password={password};Database={database};";

var conn = new MySqlConnection(connString);
var accountRepo = new AccountRepo(conn);
var transactionRepo = new TransactionRepo(conn);

builder.Services.AddTransient(_ => accountRepo);
builder.Services.AddTransient(_ => transactionRepo);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var accRep = scope.ServiceProvider.GetRequiredService<AccountRepo>();
    await accRep.Migrate();
    await transactionRepo.Migrate();
}

app.MapGet("/api/status", Handlers.Status);

app.MapGet("/api/accounts", Handlers.GetAccounts);
app.MapGet("/api/accounts/{id:int}", Handlers.GetAccountById);
app.MapPost("/api/accounts", Handlers.RegisterAccount);
app.MapPost("/api/accounts/withdraw", Handlers.WithdrawFromAccount);
app.MapPost("/api/accounts/deposit", Handlers.DepositToAccount);

app.MapGet("/api/transactions", Handlers.GetTransactions);

app.Run();
