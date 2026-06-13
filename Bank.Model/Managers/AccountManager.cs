using Bank.Model.DAL;
using Bank.Model.Entities;
using Bank.Model.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Managers
{
    public class AccountManager
    {
        private readonly Logger _logger;
        private readonly AccountDAL _dal;
        private List<Account> _accountCache;

        public AccountManager(string connectionString, Logger logger)
        {
            _dal = new AccountDAL(connectionString);
            _accountCache = new List<Account>();
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInfo("Initializing Account cache from Database.");
            _accountCache = _dal.GetAccounts();
        }

        public IReadOnlyList<Account> GetAccounts() => _accountCache.AsReadOnly(); 

        public int GetTotalAccounts()
        {
            return _accountCache.Count();
        }

        public int OpenAccount(Account newAccount)
        {
            try
            {
                _accountCache.Add(newAccount);
                _logger.LogInfo($"Successfully opened {newAccount.AccountType} account for Customer {newAccount.CustomerId} (Account ID: {newAccount.Id})");
                return _dal.OpenAccount(newAccount);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Database error in opening a new Account for Customr with CustomerId: {newAccount.CustomerId}", ex);
                throw;
            }
        }

        public void DeleteAccount(int accountId)
        {
            var cachedAccount = _accountCache.FirstOrDefault(a => a.Id == accountId);
            
            if (cachedAccount == null)
            {
                string errorMsg = $"There's no account with Id: {accountId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            try
            {
                _accountCache.Remove(cachedAccount);
                _dal.DeleteAccount(accountId);
                _logger.LogInfo($"Account with ID: {accountId} is deleted successfully!");
            }
            catch(Exception ex)
            {
                string errorMsg = $"Database issue on deleting account with Id: {accountId}";
                _logger.LogError(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }
        }

        public decimal GetBalance(int accountId)
        {
            Account account = _accountCache.FirstOrDefault(a => a.Id == accountId);
            if(account == null)
            {
                string errorMsg = $"No Account with Id: {accountId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }
            return account.Balance;
        }

        public IReadOnlyList<Account> GetAccountsByType(AccountType accountType)
        {
            return _accountCache.Where(a => a.AccountType == accountType).ToList().AsReadOnly();
        }

        public IReadOnlyList<Account> GetAccountsPerCustomer(int customerId)
        {
            return _accountCache.Where(a => a.CustomerId == customerId).ToList().AsReadOnly();
        }
        public int GetTotalAccountsByType(AccountType accountType)
        {
            return _accountCache.Count(a => a.AccountType == accountType);
        }

        public void PerformTransaction(int accountId, decimal amount, TransactionType transactionType)
        {
            var account = _accountCache.FirstOrDefault(a => a.Id == accountId);
            if(account == null)
            {
                string errorMsg = $"No Account with Id: {accountId}";
                _logger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }

            if(transactionType == TransactionType.Depoist)
            {
                account.Balance += amount;
            }
            else
            {
                if(account.Balance < amount)
                {
                    string errorMsg = $"No Account with Id: {accountId}";
                    _logger.LogError(errorMsg);
                    throw new ArgumentException(errorMsg);
                }
                account.Balance -= amount;
            }

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                TransactionDate = DateTime.Now,
                TransactionType = transactionType
            };

            try
            {
                _dal.UpdateBalance(accountId, account.Balance);
                //TODO: Insert a new record into transaction table
                _logger.LogTransaction(transactionType, accountId, amount);
            }
            catch(Exception ex)
            {
                string errorMsg = $"Database error on performing {transactionType} transaction for account {accountId}";
                if (transactionType == TransactionType.Depoist) account.Balance -= amount;
                else account.Balance += amount;
                _logger.LogError(errorMsg, ex);
            }
        }

    }
}
