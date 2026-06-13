using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Model.DAL;
using Bank.Model.Entities;
using Bank.Model.Logging;

namespace Bank.Model.Managers
{
    public class BankSystem
    {
        private static Lazy<BankSystem> _instance;
        private readonly Logger _logger;
        private readonly CustomerManager _customerManager;
        private readonly AccountManager _accountManager;

        //Hidden constructor to avoid creation of multiple objects
        private BankSystem(string connectionString)
        {
            _logger = new Logger();
            _customerManager = new CustomerManager(connectionString, _logger);
            _accountManager = new AccountManager(connectionString, _logger);
        }

        public static void Initialize(string connString)
        {
            if(_instance != null)
            {
                //Refreshing cached data
                _instance.Value._customerManager.Initialize();
                _instance.Value._accountManager.Initialize();
                string errorMsg = "BankSystem is already initialized";
                _instance.Value._logger.LogError(errorMsg);
                return;
            }

            _instance = new Lazy<BankSystem>(() => new BankSystem(connString));
            // Force first-time database load on creation
            _instance.Value._logger.LogInfo("Banking System Initialization Started");
            _instance.Value._customerManager.Initialize();
            _instance.Value._accountManager.Initialize();
            //TODO: initialize other managers.
            _instance.Value._logger.LogInfo("Banking System Initialization Completed");
        }

        //Singleton object to be used globally after calling Initialize function
        public static BankSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("BankSystem has not been initialized. Call Initialize() first.");
                }

                return _instance.Value;
            }
        }


        // --- Customers ---
        public IReadOnlyList<Customer> GetAllCustomers() => _customerManager.GetCustomers();

        public Customer RegisterCustomer(string name, int age, Gender gender, string address, string nationalId, string phoneNumber)
        {
            Customer newCustomer = new Customer
            {
                Name = name,
                Age = age,
                Gender = gender,
                Address = address,
                NationalId = nationalId,
                PhoneNumber = phoneNumber
            };
            return _customerManager.RegisterCustomer(newCustomer);
        }

        public void UpdateCustomer(int customerId, string newName, int newAge, Gender newGender, string newAddress, string newNationalId, string newPhoneNumber)
        {
            Customer customer = new Customer
            {
                Id = customerId,
                Name = newName,
                Age = newAge,
                Gender = newGender,
                Address = newAddress,
                NationalId = newNationalId,
                PhoneNumber = newPhoneNumber
            };
            _customerManager.EditCustomerInfo(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            _customerManager.DeleteCustomer(customerId);
            // TODO: refresh Accounts and services after deleting their customer (holder)
        }

        // --- Accounts ---
        public IReadOnlyList<Account> GetAllAccounts() => _accountManager.GetAccounts();
        public int GetTotalAccounts()
        {
            return _accountManager.GetTotalAccounts();
        }
        public int OpenAccount(int customerId, AccountType accountType, decimal initialBalance, DateTime createdAt)
        {
            Account account = accountType == AccountType.Salary ? (Account)new SalaryAccount() : new SavingAccount();

            account.CustomerId = customerId;
            account.AccountType = accountType;
            account.Balance = initialBalance;
            account.CreatedAt = createdAt;

            return _accountManager.OpenAccount(account);
        }
        public void DeleteAccount(int accountId)
        {
            _accountManager.DeleteAccount(accountId);
        }
        public decimal GetBalance(int accountId)
        {
            return _accountManager.GetBalance(accountId);
        }
        public IReadOnlyList<Account> GetSalaryAccounts()
        {
            return _accountManager.GetAccountsByType(AccountType.Salary);
        }
        public IReadOnlyList<Account> GetSavingAccounts()
        {
            return _accountManager.GetAccountsByType(AccountType.Saving);
        }
        public IReadOnlyList<Account> GetAccountsPerCustomer(int customerId)
        {
            return _accountManager.GetAccountsPerCustomer(customerId);
        }
        public int GetTotalSalaryAccounts()
        {
            return _accountManager.GetTotalAccountsByType(AccountType.Salary);
        }
        public int GetTotalSavingAccounts()
        {
            return _accountManager.GetTotalAccountsByType(AccountType.Saving);
        }
        public void Depoist(int accountId, decimal amount)
        {
            _accountManager.PerformTransaction(accountId, amount, TransactionType.Depoist);       
        }
        public void Withdraw(int accountId, decimal amount)
        {
            _accountManager.PerformTransaction(accountId, amount, TransactionType.Withdrawal);
        }

        // --- Transactions ---
        public IReadOnlyList<Transaction> GetTransactions() => _accountManager.GetTransactions();
        public int GetTotalTransactions()
        {
            return _accountManager.GetTotalTransactions();
        }
        public IReadOnlyList<Transaction> GetTransactionsPerAccount(int accountId)
        {
            return _accountManager.GetTransactionsPerAccount(accountId);
        }

        public int GetTotalTransactionsPerAccount(int accountId)
        {
            return _accountManager.GetTotalTransactionsPerAccount(accountId);
        }

    }
}
