using BankingSystem.Model.Contracts;
using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BankingSystem.Data.Services
{
    public class ReportingService : IReportingService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public ReportingService(AppDbContext appDbContext, ILogger logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return _appDbContext.Transactions.ToList();
        }

        public IEnumerable<Transaction> GetCustomerTransactions(int customerId)
        {
            try
            {
                bool hasCustomerLinked = _appDbContext.Accounts.Any(a => a.CustomerId == customerId);
                if (!hasCustomerLinked)
                {
                    throw new ArgumentNullException($"There's no accounts linked to a customer with CustomerId {customerId}");
                }

                return _appDbContext.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting customer transactions!", ex);
                return null;
            }
        }
        public IEnumerable<Account> GetCustomerAccounts(int customerId)
        {
            try
            {
                bool hasCustomerLinked = _appDbContext.Accounts.Any(a => a.CustomerId == customerId);
                if (!hasCustomerLinked)
                {
                    throw new ArgumentNullException($"There's no accounts linked to a customer with CustomerId {customerId}");
                }

                return _appDbContext.Accounts
                    .Include(a => a.Customer)
                    .Where(a => a.CustomerId == customerId)
                    .OrderByDescending(a => a.Customer.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting customer accounts!", ex);
                return null;
            }
        }
        public IEnumerable<BankService> GetCustomerServices(int customerId)
        {
            try
            {
                bool hasCustomerLinked = _appDbContext.BankServices.Any(a => a.CustomerId == customerId);
                if (!hasCustomerLinked)
                {
                    throw new ArgumentNullException($"There's no services linked to a customer with CustomerId {customerId}");
                }

                return _appDbContext.BankServices
                    .Include(s => s.Customer)
                    .Where(s => s.CustomerId == customerId)
                    .OrderByDescending(a => a.Customer.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting customer services!", ex);
                return null;
            }
        }

        public decimal GetCustomerTotalBalance(int customerId)
        {
            decimal balance = 0;
            try
            {
                bool hasCustomerLinked = _appDbContext.Accounts.Any(a => a.CustomerId == customerId);
                if (!hasCustomerLinked)
                {
                    throw new ArgumentNullException($"There's no accounts linked to a customer with CustomerId {customerId}");
                }

                IEnumerable<Account> accounts = _appDbContext.Accounts.Where(a => a.CustomerId == customerId);
                foreach(Account account in accounts)
                {
                    balance += account.Balance;
                }
                return balance;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting customer total balance!", ex);
                return 0.0m;
            }
        }
        public decimal GetTotalAssets()
        {
            decimal accountsSum = _appDbContext.Accounts.Select(a => (decimal?)a.Balance).Sum() ?? 0;
            decimal certificatesSum = _appDbContext.BankServices.OfType<Certificate>().Select(c => (decimal?)c.PrincipalAmount).Sum() ?? 0;
            decimal creditCardsSum = _appDbContext.BankServices.OfType<CreditCard>().Select(c => (decimal?)c.CashLimit).Sum() ?? 0;
            
            return accountsSum + certificatesSum + creditCardsSum;
        }
        public int GetTotalCustomers()
        {
            return _appDbContext.Customers.Count();
        }

        public int GetTotalSalaryAccounts()
        {
            return _appDbContext.SalaryAccounts.Include(s => s.Customer).Count();
        }
        public int GetTotalSavingAccounts()
        {
            return _appDbContext.SavingAccounts.Include(s => s.Customer).Count();
        }

        public int GetTotalCreditCards()
        {
            return _appDbContext.BankServices.OfType<CreditCard>().Include(c => c.Customer).Count();
        }
        public int GetTotalCertificates()
        {
            return _appDbContext.BankServices.OfType<Certificate>().Include(c => c.Customer).Count();
        }

        public int GetTotalTransactions()
        {
            return _appDbContext.Transactions.Count();
        }
        public int GetTotalTransactionPerCustomer(int customerId)
        {
            return GetCustomerTransactions(customerId).Count();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _appDbContext.Customers.ToList();
        }
        public IEnumerable<Account> GetAccounts()
        {
            return _appDbContext.Accounts.Include(a => a.Customer).ToList();
        }

        public IEnumerable<SalaryAccount> GetSalaryAccounts()
        {
            return _appDbContext.Accounts.OfType<SalaryAccount>().Include(s => s.Customer).ToList();
        }
        public IEnumerable<SavingAccount> GetSavingAccounts()
        {
            return _appDbContext.Accounts.OfType<SavingAccount>().Include(s => s.Customer).ToList();
        }

        public IEnumerable<BankService> GetServices()
        {
            return _appDbContext.BankServices.Include(s => s.Customer).ToList();
        }
        public IEnumerable<CreditCard> GetCreditCards()
        {
            return _appDbContext.BankServices.OfType<CreditCard>().Include(s => s.Customer).ToList();
        }
        public IEnumerable<Certificate> GetCertificates()
        {
            return _appDbContext.BankServices.OfType<Certificate>().Include(s => s.Customer).ToList();
        }
    }
}
