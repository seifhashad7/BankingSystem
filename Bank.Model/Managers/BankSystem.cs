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
        private readonly BankServiceManager _serviceManager;

        //Hidden constructor to avoid creation of multiple objects
        private BankSystem(string connectionString)
        {
            _logger = new Logger();
            _customerManager = new CustomerManager(connectionString, _logger);
            _accountManager = new AccountManager(connectionString, _logger);
            _serviceManager = new BankServiceManager(connectionString, _logger);
        }

        public static void Initialize(string connString)
        {
            if(_instance != null)
            {
                //Refreshing cached data
                _instance.Value._customerManager.Initialize();
                _instance.Value._accountManager.Initialize();
                _instance.Value._serviceManager.Initialize();
                string errorMsg = "BankSystem is already initialized";
                _instance.Value._logger.LogError(errorMsg);
                return;
            }

            _instance = new Lazy<BankSystem>(() => new BankSystem(connString));
            // Force first-time database load on creation
            _instance.Value._logger.LogInfo("Banking System Initialization Started");
            _instance.Value._customerManager.Initialize();
            _instance.Value._accountManager.Initialize();
            _instance.Value._serviceManager.Initialize();
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

        private void EnsureCustomerExists(int customerId)
        {
            if (!_customerManager.GetCustomers().Any(c => c.Id == customerId))
            {
                throw new ArgumentException($"Customer with ID {customerId} does not exist.");
            }
        }

        private void EnsureAccountExists(int accountId)
        {
            if (!_accountManager.GetAccounts().Any(a => a.Id == accountId))
            {
                throw new ArgumentException($"Account with ID {accountId} does not exist.");
            }
        }

        // --- Customers ---
        public IReadOnlyList<Customer> GetAllCustomers() => _customerManager.GetCustomers();
        public int GetTotalCustomers() => _customerManager.GetTotalCustomers();
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
            EnsureCustomerExists(customerId);
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
            EnsureCustomerExists(customerId);
            _customerManager.DeleteCustomer(customerId);
            // TODO: refresh Accounts and services after deleting their customer (holder)
        }

        // --- Accounts ---
        public IReadOnlyList<Account> GetAllAccounts() => _accountManager.GetAccounts();
        public int GetTotalAccounts()
        {
            return _accountManager.GetTotalAccounts();
        }
        public Account OpenAccount(int customerId, AccountType accountType, decimal initialBalance, DateTime createdAt)
        {
            EnsureCustomerExists(customerId);
            Account account = accountType == AccountType.Salary ? (Account)new SalaryAccount() : new SavingAccount();

            account.CustomerId = customerId;
            account.AccountType = accountType;
            account.Balance = initialBalance;
            account.CreatedAt = createdAt;

            return _accountManager.OpenAccount(account);
        }
        public void DeleteAccount(int accountId)
        {
            EnsureAccountExists(accountId);
            _accountManager.DeleteAccount(accountId);
        }
        public decimal GetBalance(int accountId)
        {
            EnsureAccountExists(accountId);
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
            EnsureCustomerExists(customerId);
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
        public void Deposit(int accountId, decimal amount)
        {
            EnsureAccountExists(accountId);
            _accountManager.PerformTransaction(accountId, amount, TransactionType.Deposit);       
        }
        public void Withdraw(int accountId, decimal amount)
        {
            EnsureAccountExists(accountId);
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
            EnsureAccountExists(accountId);
            return _accountManager.GetTransactionsPerAccount(accountId);
        }

        public int GetTotalTransactionsPerAccount(int accountId)
        {
            EnsureAccountExists(accountId);
            return _accountManager.GetTotalTransactionsPerAccount(accountId);
        }

        // --- Bank Services ---
        public IReadOnlyList<BankService> GetServices() => _serviceManager.GetServices();
        public IReadOnlyList<CreditCard> GetCreditCards() => _serviceManager.GetCreditCards();
        public IReadOnlyList<Certificate> GetCertificates() => _serviceManager.GetCertificates();
        public int GetTotalServices() => _serviceManager.GetTotalServices();
        public int GetTotalCreditCards() => _serviceManager.GetTotalCreditCards();
        public int GetTotalCertificates() => _serviceManager.GetTotalCertificates();
        public CreditCard IssueCreditCard(int customerId, decimal cashLimit)
        {
            EnsureCustomerExists(customerId);
            return _serviceManager.IssueCreditCard(customerId, cashLimit);
        }
        public Certificate IssueCertificate(int customerId, int period, decimal principalAmount) 
        {
            EnsureCustomerExists(customerId);
            return _serviceManager.IssueCertificate(customerId, period, principalAmount);
        }
        public CreditCard UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit)
        {
            EnsureCustomerExists(customerId);
            return _serviceManager.UpdateCreditCardLimit(customerId, newCreditCardLimit);
        }
        public Certificate ModifyCertificate(int customerId, int certificateId, int newPeriod, decimal newPrice) 
        {
            EnsureCustomerExists(customerId);
            return _serviceManager.ModifyCertificate(customerId, certificateId, newPeriod, newPrice);
        } 
        public void deleteCertificate(int customerId, int certificateId) 
        {
            EnsureCustomerExists(customerId);
            _serviceManager.deleteCertificate(customerId, certificateId);
        }
        public IReadOnlyList<BankService> GetBankServicesPerCustomer(int customerId)
        {
            EnsureCustomerExists(customerId);
            return _serviceManager.GetBankServicesPerCustomer(customerId);
        }

        // --- Assets ---
        public decimal GetTotalAssets()
        {
            decimal totalBalance = 0m;
            foreach(var acc in GetAllAccounts())
            {
                totalBalance += acc.Balance;
            }
            totalBalance += _serviceManager.GetTotalServiceAssets();
            return totalBalance;
        }
    }
}
