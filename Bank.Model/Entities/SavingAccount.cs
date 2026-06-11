using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public class SavingAccount : Account
    {
        public decimal InterestRate { get; set; }
        public decimal MinimumBalance { get; set; }
        public override void OpenAccount(int customerId, decimal initialBalance)
        {

        }
        public override void CloseAccount(int accountId)
        {

        }
        public override decimal GetBalance(int accountId)
        {
            decimal test = 100m;
            return test;
        }
        public override void Depoist(int accountId, decimal amount)
        {

        }
        public override void Withdraw(int accountId, decimal amount)
        {

        }
    }
}
