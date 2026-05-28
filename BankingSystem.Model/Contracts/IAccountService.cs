using BankingSystem.Model.CrossCutting;
using BankingSystem.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Contracts
{
    public interface IAccountService
    {
        Account OpenAccount(int customerId, AccountType accountType, decimal initialBalance);
        void CloseAccount(int accountId);
        decimal GetBalance(int accountId);
        void Depoist(int accountId, decimal amount);
        void Withdraw(int accountId, decimal amount);
    }
}
