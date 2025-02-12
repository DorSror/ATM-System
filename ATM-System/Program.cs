using ATM_System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// In-memory storage of Accounts
var accounts = new SortedDictionary<uint, Account>();
for (int i = 0; i < 5; i++) // init 5 accounts
{
    Account acc = new Account(); // create new account.
    accounts[acc.Account_Number] = acc; // key - unique identifier, value - the corresponding account isntance.
}

// Implementation of the first API call.
// Attempt converting the given account number into an unsigned integer. If failed, return a BadRequest error (format mismatch).
// Otherwise, check if an account with a corresponding identifier exists. If failed, return a NotFound error (no such account was found).
// If succeeded, return a response containing the balance of the found account.
app.MapGet("/accounts/{account_number}/balance", (string account_number) => 
{
    try
    {
        uint identifier = uint.Parse(account_number);
        if (!Account.AccountExists(identifier))
            return Results.NotFound(new { detail = "Account was not found." });
        return Results.Ok(new { accounts[identifier].Balance });
    }
    catch
    {
        return Results.BadRequest(new { detail = "Invalid account number format - must be an unsigned integer." });
    }
})
.WithName("Get balance")
.WithOpenApi();

// Implementation of the second API call.
// Attempt converting the given account number into an unsigned integer. If failed, return a BadRequest error (format mismatch).
// Otherwise, check if an account with a corresponding identifier exists. If failed, return a NotFound error (no such account was found).
// If succeeded, check if the withdraw amount is greater than 0 and the balance - if so then return a corresponding bad request, otherwise withdraw the amount from the account.
app.MapPost("/accounts/{account_number}/withdraw", (string account_number, [FromBody] decimal amount) => 
{
    try
    {
        uint identifier = uint.Parse(account_number);
        Account acc;
        if (!Account.AccountExists(identifier))
            return Results.NotFound(new { detail = "Account was not found." });
        acc = accounts[identifier];
        if (amount < 0)
            return Results.BadRequest(new { detail = "Withdraw amount must be non-negative." });
        if (acc.Balance < amount)
            return Results.BadRequest(new { detail = "Cannot authorize operation - insufficient funds." });
        acc.WithdrawFunds(amount);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(new { detail = "Invalid account number format - must be an unsigned integer." });
    }
})
.WithName("Withdraw money")
.WithOpenApi();

// Implementation of the third API call.
// Attempt converting the given account number into an unsigned integer. If failed, return a BadRequest error (format mismatch).
// Otherwise, check if an account with a corresponding identifier exists. If failed, return a NotFound error (no such account was found).
// If succeeded, check if the withdraw amount is greater than 0 - if so then return a bad request, otherwise deposit the amount into the account.
app.MapPost("/accounts/{account_number}/deposit", (string account_number, [FromBody] decimal amount) => 
{
    try
    {
        uint identifier = uint.Parse(account_number);
        Account acc;
        if (!Account.AccountExists(identifier))
            return Results.NotFound(new { detail = "Account was not found." });
        acc = accounts[identifier];
        if (amount < 0)
            return Results.BadRequest(new { detail = "Deposit amount must be non-negative." });
        acc.DepositFunds(amount);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(new { detail = "Invalid account number format - must be an unsigned integer." });
    }
})
.WithName("Deposit money")
.WithOpenApi();

// Extra API calls used for testing
// Please note - these calls should be used for testing purposes only!

// Retrieve all accounts
app.MapGet("/accounts/allAccounts", () =>
{
    var accs = accounts.Select(pair =>
        new AccountSummary
        (
            pair.Value.Account_Number,
            pair.Value.Balance
        ))
        .ToArray();
    return accs;
})
.WithName("Get all accounts")
.WithOpenApi();

// Delete a specified account.
// Attempt converting the given account number into an unsigned integer. If failed, return a BadRequest error (format mismatch).
// Otherwise, check if an account with a corresponding identifier exists. If failed, return a NotFound error (no such account was found).
// If succeeded, dispose of the account and remove it from the accounts dictionary.
app.MapDelete("/accounts/{account_number}/delete", (string account_number) => 
{
    try
    {
        uint identifier = uint.Parse(account_number);
        if (!Account.AccountExists(identifier))
            return Results.NotFound(new { detail = "Account was not found." });
        accounts[identifier].Dispose();
        accounts.Remove(identifier);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(new { detail = "Invalid account number format - must be an unsigned integer." });
    }
})
.WithName("Drop all accounts")
.WithOpenApi();

// Drop all accounts - first dispose of all saved accounts, and then clear the accounts dictionary.
app.MapDelete("/accounts/dropAllAccounts", () => 
{
    foreach (var acc in accounts.Values)
        acc.Dispose();
    accounts.Clear();
    return Results.Ok();
})
.WithName("Drop all accounts")
.WithOpenApi();

app.Run();

record AccountSummary(uint identifier, decimal balance);
