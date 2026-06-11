using Bank.Model.Logging;
using Bank.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public abstract class Account
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public abstract void OpenAccount(int customerId, decimal initialBalance);
        public abstract void CloseAccount(int accountId);
        public abstract decimal GetBalance(int accountId);
        public abstract void Depoist(int accountId, decimal amount);
        public abstract void Withdraw(int accountId, decimal amount);
    }
}
 