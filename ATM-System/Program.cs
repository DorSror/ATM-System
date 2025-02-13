using ATM_System;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
// For our purposes, we will use the Swagger UI, although this is not recommended outside of development.
// We simply remove the check app.Environment.IsDevelopment() (if app is in development).
app.UseSwagger();
app.UseSwaggerUI();

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
            return Results.NotFound(APIConstants.AccountNotFound);
        return Results.Ok(accounts[identifier].Balance);
    }
    catch
    {
        return Results.BadRequest(APIConstants.InvalidIndentifierFormat);
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
            return Results.NotFound(APIConstants.AccountNotFound);
        acc = accounts[identifier];
        if (amount < 0)
            return Results.BadRequest(APIConstants.InvalidBalanceWithdraw);
        if (acc.Balance < amount)
            return Results.BadRequest(APIConstants.InsufficientFunds);
        acc.WithdrawFunds(amount);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(APIConstants.InvalidIndentifierFormat);
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
            return Results.NotFound(APIConstants.AccountNotFound);
        acc = accounts[identifier];
        if (amount < 0)
            return Results.BadRequest(APIConstants.InvalidBalanceDeposit);
        acc.DepositFunds(amount);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(APIConstants.InvalidIndentifierFormat );
    }
})
.WithName("Deposit money")
.WithOpenApi();

// Extra API calls used for testing
// Please note - some of these calls should be used for testing purposes only.

// Add an account with a (possibly) given balance. Note that we allow the creation of accounts with negative balance for testing purposes.
app.MapPost("/accounts/newAccount", ([FromBody] decimal? balance) =>
{
    Account acc = new Account(balance);
    accounts[acc.Account_Number] = acc;
    return Results.Ok();
})
.WithName("Create new account")
.WithOpenApi();

// Retrieve all accounts. (Use for testing only!)
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

// Delete a specified account. (Use for testing only!)
// Attempt converting the given account number into an unsigned integer. If failed, return a BadRequest error (format mismatch).
// Otherwise, check if an account with a corresponding identifier exists. If failed, return a NotFound error (no such account was found).
// If succeeded, dispose of the account and remove it from the accounts dictionary.
app.MapDelete("/accounts/{account_number}/delete", (string account_number) => 
{
    try
    {
        uint identifier = uint.Parse(account_number);
        if (!Account.AccountExists(identifier))
            return Results.NotFound(APIConstants.AccountNotFound);
        accounts[identifier].Dispose();
        accounts.Remove(identifier);
        return Results.Ok();
    }
    catch
    {
        return Results.BadRequest(APIConstants.InvalidIndentifierFormat);
    }
})
.WithName("Delete account")
.WithOpenApi();

// Drop all accounts - first dispose of all saved accounts, and then clear the accounts dictionary. (Use for testing only!)
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

record AccountSummary(uint Account_Number, decimal Balance);
