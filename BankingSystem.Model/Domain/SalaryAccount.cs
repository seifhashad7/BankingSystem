using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Domain
{
    public class SalaryAccount : Account
    {
        public string EmployerName { get; set; }
        public DateTime LastSalaryCreditDate { get; set; }
        public bool IsZeroBalancedAccount { get; set; }
    }
}
