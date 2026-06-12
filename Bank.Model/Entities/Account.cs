using Bank.Model.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Model.Entities
{
    public enum AccountType
    {
        Salary,
        Saving
    }
    public abstract class Account
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public AccountType AccountType { get; set; }
    }
    public class SavingAccount : Account
    {
        public decimal InterestRate { get; set; }
        public decimal MinimumBalance { get; set; }
    }
    public class SalaryAccount : Account
    {
        public string EmployerName { get; set; }
        public DateTime? LastSalaryCreditDate { get; set; }
        public bool? IsZeroBalancedAccount { get; set; }
    }
}
 