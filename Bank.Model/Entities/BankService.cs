using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public abstract class BankService
    {
        public int Id { get; set; }
        public int PeriodInYears { get; set; }
        public DateTime IssueDate { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
    public class CreditCard : BankService
    {
        public CreditCard()
        {
            PeriodInYears = 10;
        }

        public decimal CashLimit { get; set; } // 50k - 250k
    }
    public class Certificate : BankService
    {
        public decimal PrincipalAmount { get; set; } = 1000; //Multiples of 1000
        public decimal InterestRate { get; set; } //10% for 1 year, 15% for 3 years and 20% for 5 years
    }
}
