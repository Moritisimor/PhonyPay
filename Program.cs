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

builder.Services.AddMySqlDataSource(connString);
builder.Services.AddTransient<AccountRepo>();
builder.Services.AddTransient<TransactionRepo>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var accRep = scope.ServiceProvider.GetRequiredService<AccountRepo>();
    var transRep = scope.ServiceProvider.GetRequiredService<TransactionRepo>();
    
    await accRep.Migrate();
    await transRep.Migrate();
}

app.MapGet("/api/status", Handlers.Status);

app.MapGet("/api/accounts", Handlers.GetAccounts);
app.MapGet("/api/accounts/{id:int}", Handlers.GetAccountById);
app.MapPost("/api/accounts", Handlers.RegisterAccount);
app.MapPost("/api/accounts/withdraw", Handlers.WithdrawFromAccount);
app.MapPost("/api/accounts/deposit", Handlers.DepositToAccount);

app.MapGet("/api/transactions", Handlers.GetTransactions);
app.MapGet("/api/transactions/{id:int}", Handlers.GetTransactionById);
app.MapPost("/api/transactions", Handlers.PostTransaction);

app.Run();
