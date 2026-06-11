using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public class CreditCard : BankService
    {
        public CreditCard()
        {
            PeriodInYears = 10;
        }

        public decimal CashLimit { get; set; } // 50k - 250k
        public void IssueCreditCard(int customerId, decimal cashLimit)
        {

        }
        public void UpdateCreditCardLimit(int customerId, decimal newCreditCardLimit)
        {

        }
    }
}
