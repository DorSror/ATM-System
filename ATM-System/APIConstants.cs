using System;

namespace ATM_System;

/// <summary>
/// Class <c>APIConstants</c> is used to store the constant used in the API calls. 
/// </summary>
public class APIConstants
{
    public const string AccountNotFound = "404: Account was not found.";
    public const string InvalidIndentifierFormat = "400: Invalid account number format - must be an unsigned integer.";
    public const string InvalidBalanceWithdraw = "400: Withdraw amount must be non-negative.";
    public const string InvalidBalanceDeposit = "400: Deposit amount must be non-negative.";
    public const string InsufficientFunds = "400: Cannot authorize operation - insufficient funds.";

}
