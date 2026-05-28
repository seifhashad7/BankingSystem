using BankingSystem.Model.CrossCutting;
using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Contracts
{
    public interface IReportingService
    {
        IEnumerable<Transaction> GetCustomerTransactions(int customerId);
        IEnumerable<Account> GetCustomerAccounts(int customerId);
        decimal GetCustomerTotalBalance(int customerId);
        int GetTotalCustomers();
        int GetTotalCreditCards();
        int GetTotalCertificates();
        int GetTotalTransactions();

        int GetTotalTransactionPerCustomer(int customerId);

        IEnumerable<Customer> GetCustomers();
        IEnumerable<Account> GetAccounts();
        IEnumerable<BankService> GetServices();
    }
}
