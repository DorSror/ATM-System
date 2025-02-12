using System;
using System.Collections.Generic;

namespace ATM_System;

/// <summary>
/// Class <c>Account</c> used to represent bank accounts with a unique identifier and balance. 
/// <br/>Implements the <c>IDisposable</c> interface.
/// </summary>
public class Account : IDisposable
{
    /// <summary>
    /// A static HashSet used to store all unique identifiers in use, used to ensure uniqueness of new identifiers.
    /// </summary>
    private static HashSet<uint> allUniqueIdentifiers = new HashSet<uint>();

    /// <summary>
    /// Unique identifier for the account, stored as an unsigned integer (non-negative integer). 
    /// </summary>
    public uint Account_Number { get; private set; }
    
    /// <summary>
    /// The balance of the account, stored as a decimal (we prefer exact precision over approximate precision given by float/double). 
    /// </summary>
    public decimal Balance { get; private set;}

    /// <summary>
    /// Initialize a new instance of <c>Account</c> class, with a unique identifier and a starting default balance of 0.
    /// </summary>
    public Account() {
        Account_Number = GenerateUniqueIdentifier();
        Balance = 0;
    }

    /// <summary>
    /// Initialize a new instance of <c>Account</c> class, with a unique identifier and a given balance.
    /// </summary>
    /// <param name="balance"> The starting balance of the account.</param>
    public Account(decimal balance) {
        Account_Number = GenerateUniqueIdentifier();
        Balance = balance;
    }

    /// <summary>
    /// Generates a unique account number to this account.
    /// </summary>
    /// <returns>A unique unsigned integer representing the account number.</returns>
    private uint GenerateUniqueIdentifier() {
        uint New_Identifier;
        Random random = new Random();

        do {
            New_Identifier = (uint)random.Next(); // generate a number and convert to uint.
        } while (allUniqueIdentifiers.Contains(New_Identifier)); // repeat until the identifiers HashSet doesn't contain the generated number.

        allUniqueIdentifiers.Add(New_Identifier); // update the HashSet

        return New_Identifier;
    }

    /// <summary>
    /// Withdraws (deducts) funds from the account by subtracting the requested amount from the balance.
    /// </summary>
    /// <param name="amount">The requested amount to withdraw.</param>
    public void WithdrawFunds(decimal amount) {
        Balance -= amount;
    }

    /// <summary>
    /// Deposits (deducts) funds from the account by subtracting the requested amount from the balance.
    /// </summary>
    /// <param name="amount">The requested amount to withdraw.</param>
    public void DepositFunds(decimal amount) {
        Balance += amount;
    }

    /// <summary>
    /// Manually dispose of the account instance by removing the account number from the unique identifiers set.
    /// </summary>
    public void Dispose() {
        allUniqueIdentifiers.Remove(Account_Number);
    }

    /// <summary>
    /// Destructor (finalizer) that removes the account number from the unique identifiers set.
    /// </summary>
    ~Account() {
        allUniqueIdentifiers.Remove(Account_Number);
    }

    /// <summary>
    /// Returns a string representation of the account's number and balance.
    /// </summary>
    /// <returns>A formatted string containing the account details.</returns>
    public override string ToString() {
        return $"Account Number: {Account_Number}, Balance: {Balance}";
    }

    /// <summary>
    /// Checks whether a given account number is associated with an account, i.e. an account with the given unique identifier exists. 
    /// <br/> Note that the function is not safe as an attacker can easily find existing accounts, but we ignore this fact in the assignment.
    /// </summary>
    /// <param name="Identifier">The account number that will be checked.</param>
    /// <returns>True if the account number exists, otherwise false.</returns>
    public static bool AccountExists(uint Identifier) {
        return allUniqueIdentifiers.Contains(Identifier);
    }

    /// <summary>
    /// Returns all unique identifiers. <br/>SHOULD BE USED ONLY FOR TESTING PURPOSES!
    /// </summary>
    /// <returns>HashSet of all unique identifiers, in uint format.</returns>
    public static HashSet<uint> GetAllIdentifiers() {
        return allUniqueIdentifiers;
    }
}
