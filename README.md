# PhonyPay
Web API for simulating bank accounts &amp; transactions written in C#

## What is this project about?
This project is a simple web API that simulates bank accounts and transactions.

It's not meant for actual production use, instead, it's meant more for simulating bank accounts and transactions, for example, for testing/learning.

It uses a MySQL/MariaDB Database for persistence using Dapper for querying/executing statements.

## Cloning and compilation
To clone:
```bash
git clone https://github.com/Moritisimor/PhonyPay
cd PhonyPay
```

To compile it:
```bash
dotnet publish
```

Or to just run it directly:
```bash
dotnet run
```

## Environment Variables
It requires the following environment variables to be set for database connectivity to work:
- `DATABASE`
- `DB_USERNAME`
- `DB_SERVER` (The host)
- `DB_PASSWORD`

## Rest API
### /api/status
If the API is up and running, it will return a 200 OK, along with this object:
```json
{ "status": "OK" }
```

### GET /api/status/db
Checks if the database is up and running. If so, 200 OK is returned along with this object:
```json
{ "status": "OK" }
```

### GET /api/accounts
Returns a list of all accounts in the database.

This endpoint is generally not expected to return a non-200 response.

If a non-200 response is returned, it's most likely because the database is not up and running.

### GET /api/accounts/{id}
Returns the account with the given ID.

If the account is found, 200 OK is returned along with an object such as this:
```json
{
  "accountId": "integer",
  "firstName": "string",
  "lastName": "string",
  "balance": "float"
}
```

### POST /api/accounts
Creates a new account in the database.

The request body should be a JSON object with the following properties:
```json
{
  "firstName": "string",
  "lastName": "string"
}
```

If the body is bad JSON or does not contain the required properties, a 400 Bad Request is returned.

Otherwise, a 200 OK is returned along with the newly created account's ID within a JSON object such as this:
```json
{
  "id": "integer"
}
```

### POST /api/accounts/withdraw
Withdraws money from an account.

Semantically, it simply subtracts the amount from the account's balance.

It may fail for the following reasons:
- The account does not exist
- The account does not have enough money
- The amount is not a positive number

A 200 OK is returned along with the updated account's balance in a JSON object such as this:
```json
{ "newBalance": "float" }
```

### POST /api/accounts/deposit
Deposits money into an account.

It may fail if the account does not exist.

A 200 OK is returned along with the updated account's balance in a JSON object such as this:
```json
{ "newBalance": "float" }
```

### GET /api/transactions
Returns a list of all transactions in the database.

Like `/api/accounts`, this endpoint is generally not expected to return a non-200 response.

It may return a 500 Internal Server Error if the database is not up and running.

The JSON response is an array of objects such as this:
```json
[
  {
    "transactionId": "integer",
    "payerId": "integer",
    "receiverId": "integer",
    "amount": "float"
  }
]
```

### GET /api/transactions/{id}
Returns the transaction with the given ID.

A 404 Not Found may be returned if there is no transaction with the given ID.

Otherwise, a 200 OK is returned along with an object such as this:
```json
{
  "transactionId": "integer",
  "payerId": "integer",
  "receiverId": "integer",
  "amount": "float"
}
```

### POST /api/transactions
Creates a new transaction in the database.

A JSON of this schema is expected:
```json
{
  "payerId": "integer",
  "receiverId": "integer",
  "amount": "float"
}
```

A 400 Bad Request is returned if the request body is not valid JSON or does not contain the required properties.

Otherwise, a 200 OK is returned along with the newly created transaction's ID within a JSON object such as this:
```json
{ "id": "integer" }
```
