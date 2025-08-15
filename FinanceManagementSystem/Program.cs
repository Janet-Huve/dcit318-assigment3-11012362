using System;
using System.Collections.Generic;

record Transaction(int Id, DateTime Date, decimal Amount, string Category);

interface ITransactionProcessor { void Process(Transaction t); }

class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction t) =>
        Console.WriteLine($"Bank Transfer: {t.Amount} for {t.Category}");
}

class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction t) =>
        Console.WriteLine($"Mobile Money: {t.Amount} for {t.Category}");
}

class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction t) =>
        Console.WriteLine($"Crypto Payment: {t.Amount} for {t.Category}");
}

class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }
    public Account(string acc, decimal bal) { AccountNumber = acc; Balance = bal; }
    public virtual void ApplyTransaction(Transaction t) => Balance -= t.Amount;
}

sealed class SavingsAccount : Account
{
    public SavingsAccount(string acc, decimal bal) : base(acc, bal) { }
    public override void ApplyTransaction(Transaction t)
    {
        if (t.Amount > Balance) Console.WriteLine("Insufficient funds");
        else { Balance -= t.Amount; Console.WriteLine($"New Balance: {Balance}"); }
    }
}

class FinanceApp
{
    List<Transaction> _transactions = new();
    public void Run()
    {
        var acc = new SavingsAccount("ACC123", 1000);
        var t1 = new Transaction(1, DateTime.Now, 100, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 200, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 50, "Entertainment");

        new MobileMoneyProcessor().Process(t1);
        new BankTransferProcessor().Process(t2);
        new CryptoWalletProcessor().Process(t3);

        acc.ApplyTransaction(t1);
        acc.ApplyTransaction(t2);
        acc.ApplyTransaction(t3);

        _transactions.AddRange(new[] { t1, t2, t3 });
    }
    static void Main() => new FinanceApp().Run();
}

