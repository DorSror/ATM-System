using System;

namespace ATM_System;

/// <summary>
/// Class <c>APIConstants</c> is used to store the constant used in the API calls. 
/// </summary>
public class APIConstants
{
    public const string AccountNotFound = "Account was not found.";
    public const string InvalidIndentifierFormat = "Invalid account number format - must be an unsigned integer.";
    public const string InvalidBalanceWithdraw = "Withdraw amount must be non-negative.";
    public const string InvalidBalanceDeposit = "Deposit amount must be non-negative.";
    public const string InsufficientFunds = "Cannot authorize operation - insufficient funds.";

}
