using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model.Domain
{
    public class SavingAccount : Account
    {
        public decimal InterestRate { get; set; }
        public decimal MinimumBalance { get; set; }
    }
}
