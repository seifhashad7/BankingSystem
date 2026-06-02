using BankingSystem.Model;
using BankingSystem.Model.Contracts;
using BankingSystem.Model.CrossCutting;
using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Data
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        //DI of DbContext in AccountService ctor
        public AccountService(AppDbContext appDbContext, ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        } 

        public Account OpenAccount(int customerId, AccountType accountType, decimal initialBalance)
        {
            try
            {
                if(_appDbContext.Customers.Find(customerId) == null)
                {
                    throw new Exception("Customer with id" + customerId + " not found!");
                }

                Account newAccount = (accountType == AccountType.Saving) ? (Account) new SavingAccount() : new SalaryAccount();

                newAccount.CustomerId = customerId;
                newAccount.CreatedAt = DateTime.Now;

                _appDbContext.Accounts.Add(newAccount);
                _appDbContext.SaveChanges();

                if(initialBalance > 0)
                {
                    Depoist(newAccount.Id, initialBalance);
                }

                _logger.LogInfo($"Account opened for CustomerId: {customerId}, Account type: {accountType}");

                return newAccount;
            }
            catch(Exception e)
            {
                _logger.LogError("Error opening account", e);
                return null;
            }
        }

        public void CloseAccount(int accountId)
        {

            try
            {
                var accountToBeClosed = _appDbContext.Accounts.Find(accountId);
                if(accountToBeClosed == null)
                {
                    throw new Exception($"Account with this ID: {accountId} is not found");
                }

                _appDbContext.Accounts.Remove(accountToBeClosed);
                _appDbContext.SaveChanges();

                _logger.LogInfo($"Account with ID: {accountId} is closed!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error closing account", ex);
            }
            
        }

        public decimal GetBalance(int accountId)
        {
            try
            {
                var account = _appDbContext.Accounts.Find(accountId);
                if (account == null)
                {
                    throw new Exception($"Account with this ID: {accountId} is not found");
                }
                return account.Balance;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error Getting Balance", ex);
                return -1;
            }
        }

        public void Depoist(int accountId, decimal amount)
        {
            try
            {
                var account = _appDbContext.Accounts.Find(accountId);
                if(account == null)
                {
                    throw new Exception($"Account with provided ID: {accountId} is not found!");
                }

                account.Balance += amount;

                var depoistTransaction = new Transaction
                {
                    AccountId = accountId,
                    Amount = amount,
                    TransactionType = TransactionType.Depoist,
                    TransactionDate = DateTime.Now
                };

                _appDbContext.Transactions.Add(depoistTransaction);
                _appDbContext.SaveChanges();

                _logger.LogTransaction(TransactionType.Depoist, accountId, amount);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error depoisting the amount", ex);
            }
        }

        public void Withdraw(int accountId, decimal amount)
        {
            try
            {
                var account = _appDbContext.Accounts.Find(accountId);
                if (account == null)
                {
                    throw new Exception($"Account with provided ID: {accountId} is not found!");
                }

                if(amount > account.Balance)
                {
                    throw new ArgumentOutOfRangeException($"Amount: {amount} exceeds account's balance");
                }

                account.Balance -= amount;

                var withdrawTransaction = new Transaction
                {
                    AccountId = accountId,
                    Amount = amount,
                    TransactionType = TransactionType.Withdrawal,
                    TransactionDate = DateTime.Now
                };

                _appDbContext.Transactions.Add(withdrawTransaction);
                _appDbContext.SaveChanges();

                _logger.LogTransaction(TransactionType.Withdrawal, accountId, amount);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error withdrawing the amount", ex);
            }
        }
    }
}
